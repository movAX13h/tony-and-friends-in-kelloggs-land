using Kelloggs.Controls;
using Kelloggs.Formats;
using Kelloggs.Tool;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kelloggs
{
    public partial class Form1 : Form
    {
        private const string baseTitle = "Kelloggs Tools"; 
        private const string filename = "../../../Kelloggs/PCKELL.DAT";

        private DATFile container;

        public Form1()
        {
            InitializeComponent();
            Text = baseTitle;
            datFileEntriesListView.SelectedIndexChanged += datFileEntriesListView_SelectedIndexChanged;
            datFileEntriesListView.ListViewItemSorter = new ListViewColumnSorter();

            //detailsPanel.DoubleBuffered(true);
            //imgPictureBox.DoubleBuffered(true);

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
            if (datFileEntriesListView.SelectedItems.Count == 0) return;
            displayFileDetails(datFileEntriesListView.SelectedItems[0].Tag as DATFileEntry);
        }

        private void displayFileDetails(DATFileEntry entry)
        {
            Text = baseTitle + " - " + entry.Filename;

            switch (entry.Type)
            {
                case "BOB":
                    BOBFile b = new BOBFile(entry, Palette.Default);
                    if (b.Error != "")
                    {
                        MessageBox.Show(b.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    log($"BOB contains {b.Elements.Count} images");
                    imgPictureBox.Image = BitmapScaler.PixelScale(BOBPainter.MakeSheet(b), 3);
                    setPaletteImage(Palette.Default);
                    break;

                case "ICO":
                    //TODO: better error handling
                    ICOFile ico = new ICOFile(entry, Palette.Default);
                    if (ico.Error != "")
                    {
                        MessageBox.Show("Failed to load ICO, sorry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    log($"ICO contains {ico.Bitmaps.Length} tiles");                    
                    imgPictureBox.Image = BitmapScaler.PixelScale(ICOPainter.TileSetFromBitmaps(ico.Bitmaps), 3);
                    setPaletteImage(Palette.Default);
                    break;

                case "MAP":
                    MAPFile map = new MAPFile(entry);
                    if (map.Error != "")
                    {
                        MessageBox.Show(map.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Palette palette = Palette.Default; //getPaletteFrom("FAC0.PCC");

                    Bitmap mapBitmap = null;

                    // find matching ICO file
                    if (entry.Filename.StartsWith("W"))
                    {
                        string icoName = entry.Filename.Substring(0, 2) + ".ICO";
                        if (container.Entries.ContainsKey(icoName))
                        {
                            ICOFile icoFile = new ICOFile(container.Entries[icoName], palette);
                            mapBitmap = MAPPainter.Paint(map, icoFile, 1);
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

                case "PCC": // these are actually PCX files, version 5, encoded, 8 bits per pixel
                    PCXFile pcx = new PCXFile();
                    if (!pcx.Load(entry.Data))
                    {
                        MessageBox.Show(pcx.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    imgPictureBox.Image = BitmapScaler.PixelScale(pcx.Bitmap, 4);
                    setPaletteImage(Palette.ToBitmap(pcx.Palette), "own");
                    log($"PCC loaded: name={entry.Filename}, width={pcx.Bitmap.Width}, height={pcx.Bitmap.Height}, palette=own");
                    break;

                default:
                    break;
            }
        }
        #endregion

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
            if (pcx.Load(container.Entries[pccName].Data)) return new Palette(pcx.Palette);
            return null;
        }

        private void log(string message)
        {
            outputBox.AppendText(message + Environment.NewLine);
        }

        #region buttons
       
        private void exportButton_Click(object sender, EventArgs e)
        {
            if (!container.Ready) return;
            container.ExportAll();
        }
        #endregion

        private void detailsPanel_Scroll(object sender, ScrollEventArgs e)
        {
            detailsPanel.Invalidate();
            imgPictureBox.Invalidate();
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
    }
}
