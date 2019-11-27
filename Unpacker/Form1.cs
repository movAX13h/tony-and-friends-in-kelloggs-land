using Kelloggs.Controls;
using Kelloggs.Formats;
using Kelloggs.Tool;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;
using System.Drawing.Imaging;

namespace Kelloggs
{
    public partial class Form1 : Form
    {
        private const string baseTitle = "Kelloggs Tools"; 
        private const string filename = "../../../Kelloggs/PCKELL.DAT";

        private DATFile container;
        private Notes notes;

        private Bitmap currentExportBitmap;
        private Bitmap[] currentExportBitmaps;

        public Form1()
        {
            InitializeComponent();
            Text = baseTitle;
            datFileEntriesListView.SelectedIndexChanged += datFileEntriesListView_SelectedIndexChanged;
            datFileEntriesListView.ListViewItemSorter = new ListViewColumnSorter();

            //detailsPanel.DoubleBuffered(true);
            //imgPictureBox.DoubleBuffered(true);

            notes = new Notes("notes.txt");
            
            if (loadContainer())
            {
                datFileEntriesListView.Items[41].Selected = true;
            }
        }

        private bool loadContainer()
        {
            container = new DATFile(filename);

            if (!container.Parse())
            {
                MessageBox.Show("Sorry, seems like the DAT file is different from what was expected." + Environment.NewLine +
                "Check log output for details.", "Failed to parse DAT file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                outputBox.Text = container.Log;
                return false;
            }

            outputBox.Text = container.Log;
            foreach (DATFileEntry entry in container.Entries.Values)
            {
                entry.Note = notes.GetNote(entry.Filename);

                var item = new ListViewItem(new string[] { entry.Filename, entry.TypeName, entry.Note });
                item.Tag = entry;
                item.UseItemStyleForSubItems = false;
                item.SubItems[1].BackColor = entry.TypeColor;
                datFileEntriesListView.Items.Add(item);
            }

            return true;
        }

        #region file list & details
        private void datFileEntriesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (datFileEntriesListView.SelectedItems.Count == 0)
            {
                saveSelectedButton.Enabled = false;
                return;
            }

            saveSelectedButton.Enabled = true;
            displayFileDetails(datFileEntriesListView.SelectedItems[0].Tag as DATFileEntry);
        }

        private void displayFileDetails(DATFileEntry entry)
        {
            Text = baseTitle + " - " + entry.Filename;

            savePNGButton.Visible = false;
            savePNGSButton.Visible = false;

            currentExportBitmap = null;
            currentExportBitmaps = null;

            switch (entry.Type)
            {
                case "MAP":
                    MAPFile map = new MAPFile(entry);
                    if (map.Error != "")
                    {
                        MessageBox.Show(map.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Palette palette = Palette.Default;
                    Bitmap mapBitmap = null;

                    // find matching ICO tileset and PCC for palette
                    if (entry.Filename.StartsWith("W"))
                    {
                        string baseName = entry.Filename.Substring(0, 2);
                        if (container.Entries.ContainsKey(baseName + ".ICO"))
                        {
                            palette = getPaletteFrom(baseName + ".PCC");
                            if (palette == null) palette = Palette.Default;
                            else if (baseName != "W2") addW2Palette(palette);

                            ICOFile icoFile = new ICOFile(container.Entries[baseName + ".ICO"], palette);
                            int scale = 1;
                            mapBitmap = MAPPainter.Paint(map, icoFile, scale);
                            currentExportBitmap = scale == 1 ? mapBitmap : MAPPainter.Paint(map, icoFile, 1);
                        }
                    }

                    imgPictureBox.Image = mapBitmap;
                    setPaletteImage(palette);
                    log($"MAP loaded: name={entry.Filename}, width={map.Width}, height={map.Height}, palette={palette.Name}");
                    break;

                case "ARE":
                    AREFile area = new AREFile(entry);
                    if (area.Error != "")
                    {
                        MessageBox.Show(area.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //imgPictureBox.Image = BitmapScaler.PixelScale(, 3);
                    //palettePictureBox.Image = BitmapScaler.PixelScale(Palette.ToBitmap(Palette.Default), 6);
                    break;

                case "BOB":
                    string name = Path.GetFileNameWithoutExtension(entry.Filename);
                    if (container.Entries.ContainsKey(name + ".PCC")) palette = getPaletteFrom(name + ".PCC");
                    else if (name.Length == 1) palette = getPaletteFrom("ANTS.PCC"); // the ants have names like A.BOB to O.BOB
                    else
                    {
                        palette = getPaletteFrom("W2.PCC");
                        palette.Colors[0] = Color.Black;
                        palette.Name += " (mod)";
                    }

                    BOBFile b = new BOBFile(entry, palette);
                    if (b.Error != "")
                    {
                        MessageBox.Show(b.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    log($"BOB contains {b.Elements.Count} images");
                    currentExportBitmaps = b.Elements.ToArray();
                    currentExportBitmap = BOBPainter.MakeSheet(b);
                    imgPictureBox.Image = BitmapScaler.PixelScale(currentExportBitmap, 3);
                    setPaletteImage(palette);
                    break;

                case "ICO": // tileset / spritesheet
                    string world = entry.Filename.Contains("W1") ? "W1" : (entry.Filename.Contains("W2") ? "W2" : "W3");
                    palette = getPaletteFrom(world + ".PCC");
                    if (palette == null) palette = Palette.Default;
                    else if (world != "W2") addW2Palette(palette);

                    ICOFile ico = new ICOFile(entry, palette);
                    if (ico.Error != "")
                    {
                        MessageBox.Show("Failed to load ICO, sorry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    log($"ICO contains {ico.Bitmaps.Length} tiles");
                    currentExportBitmaps = ico.Bitmaps;
                    currentExportBitmap = ICOPainter.TileSetFromBitmaps(ico.Bitmaps);
                    imgPictureBox.Image = BitmapScaler.PixelScale(currentExportBitmap, 3);
                    setPaletteImage(palette);
                    break;

                
                case "PCC": // these are actually PCX files, version 5, encoded, 8 bits per pixel
                    PCXFile pcx = new PCXFile();
                    if (!pcx.Load(entry.Data))
                    {
                        MessageBox.Show(pcx.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    currentExportBitmap = pcx.Bitmap;
                    imgPictureBox.Image = BitmapScaler.PixelScale(pcx.Bitmap, 4);
                    setPaletteImage(Palette.ToBitmap(pcx.Palette), "own");
                    log($"PCC loaded: name={entry.Filename}, width={pcx.Bitmap.Width}, height={pcx.Bitmap.Height}, palette=own");
                    break;

                default:
                    break;
            }

            if (currentExportBitmap != null) savePNGButton.Visible = true;
            if (currentExportBitmaps != null && currentExportBitmaps.Length > 0) savePNGSButton.Visible = true;
        }
        
        private void datFileEntriesListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListViewColumnSorter sorter = datFileEntriesListView.ListViewItemSorter as ListViewColumnSorter;

            if (e.Column == sorter.SortColumn)
            {
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                sorter.SortColumn = e.Column;
                sorter.Order = SortOrder.Ascending;
            }

            datFileEntriesListView.Sort();
        }
        #endregion

        #region palette
        // we don't have the correct complete palette for all ICO files
        // there are 16 colors in W2.PCC which seem to be used for animations and items for all levels
        // so we copy them into W1 or W3 here
        private void addW2Palette(Palette palette)
        {
            Palette paletteW2 = getPaletteFrom("W2.PCC");

            for (int i = 0; i < 16; i++)
                palette.Colors[i + 16] = paletteW2.Colors[i + 16];

            palette.Name += " (mod)";
        }

        private void setPaletteImage(Palette palette)
        {
            setPaletteImage(Palette.ToBitmap(palette), palette.Name);
        }

        private void setPaletteImage(Bitmap bmp, string name)
        {
            palettePictureBox.Image = BitmapScaler.PixelScale(bmp, 8);
            paletteNameLabel.Text = name;
        }

        private Palette getPaletteFrom(string pccName)
        {
            var pcx = new PCXFile();
            if (pcx.Load(container.Entries[pccName].Data))
            {
                Palette palette = new Palette(pcx.Palette);
                palette.Name = pccName;
                return palette;
            }
            return null;
        }
        #endregion

        #region helpers
        private void showFileInFolder(string path)
        {
            if (!File.Exists(path)) return;
            string argument = "/select, \"" + path + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        private void log(string message)
        {
            outputBox.AppendText(message + Environment.NewLine);
        }
        #endregion

        #region buttons       
        private void exportButton_Click(object sender, EventArgs e)
        {
            if (!container.Ready) return;
            container.ExportAll();
            string file = container.Entries["A.BOB"].ExportPath;
            showFileInFolder(file);
            log("all assets saved to " + Path.GetDirectoryName(file));
        }
        
        private void saveSelectedButton_Click(object sender, EventArgs e)
        {
            if (datFileEntriesListView.SelectedItems.Count > 0)
            {
                var entry = (DATFileEntry)datFileEntriesListView.SelectedItems[0].Tag;
                string file = entry.SaveToDisk();
                showFileInFolder(file);
                log(entry.Filename + " saved to " + Path.GetFullPath(file));
            }
        }
        
        private void savePNGButton_Click(object sender, EventArgs e)
        {
            var entry = (DATFileEntry)datFileEntriesListView.SelectedItems[0].Tag;
            using (SaveFileDialog savePNGDialog = new SaveFileDialog())
            {
                savePNGDialog.RestoreDirectory = true;
                savePNGDialog.FileName = entry.Filename + ".png";
                if (savePNGDialog.ShowDialog() == DialogResult.OK)
                {
                    currentExportBitmap.Save(savePNGDialog.FileName);
                    showFileInFolder(savePNGDialog.FileName);
                }
            }
        }
        
        private void savePNGSButton_Click(object sender, EventArgs e)
        {
            var entry = (DATFileEntry)datFileEntriesListView.SelectedItems[0].Tag;
            using (SaveFileDialog savePNGSDialog = new SaveFileDialog())
            {
                savePNGSDialog.RestoreDirectory = true;
                savePNGSDialog.FileName = entry.Filename + ".zip";
                if (savePNGSDialog.ShowDialog() == DialogResult.OK)
                {
                    string file = savePNGSDialog.FileName;
                    if (File.Exists(file)) File.Delete(file);
                    using (ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Create))
                    {
                        int num = 1;
                        foreach (Bitmap bitmap in currentExportBitmaps)
                        {
                            var zipEntry = zip.CreateEntry($"{entry.Filename}_{num}.png", CompressionLevel.Fastest);
                            using (Stream stream = zipEntry.Open())
                            {
                                bitmap.Save(stream, ImageFormat.Png);
                            }
                            
                            num++;
                        }
                    }

                    showFileInFolder(file);
                }
            }
        }
        #endregion

        private void detailsPanel_Scroll(object sender, ScrollEventArgs e)
        {
            detailsPanel.Invalidate();
            imgPictureBox.Invalidate();
        }

    }
}
