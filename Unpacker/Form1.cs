using Kelloggs.Formats;
using Kelloggs.Tool;
using System;
using System.Collections.Generic;
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
        }

        private void loadContainer()
        {
            container = new DATFile(filename);

            if (!container.Parse())
            {
                MessageBox.Show("Sorry, seems like the DAT file is different from what was expected." + Environment.NewLine +
                "Check log output for details.", "Failed to parse DAT file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                outputBox.Text = container.Log;
                return;
            }

            outputBox.Text = container.Log;
            foreach (DATFileEntry entry in container.Entries.Values)
            {
                var item = new ListViewItem(new string[] { entry.Filename, entry.TypeName, entry.Note });
                item.Tag = entry;
                datFileEntriesListView.Items.Add(item);
            }
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
                    
                    imgPictureBox.Image = BitmapScaler.PixelScale(BOBPainter.MakeSheet(b), 3);
                    palettePictureBox.Image = BitmapScaler.PixelScale(Palette.ToBitmap(Palette.Default), 6);
                    break;

                case "ICO":
                    //TODO: better error handling
                    ICOFile ico = new ICOFile(entry, Palette.Default);
                    if (ico.Error != "")
                    {
                        MessageBox.Show("Failed to load ICO, sorry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    outputBox.AppendText($"ICO contains {ico.Bitmaps.Length} tiles" + Environment.NewLine);
                    imgPictureBox.Image = BitmapScaler.PixelScale(ICOPainter.TileSetFromBitmaps(ico.Bitmaps), 3);
                    palettePictureBox.Image = BitmapScaler.PixelScale(Palette.ToBitmap(Palette.Default), 6);
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
                    palettePictureBox.Image = BitmapScaler.PixelScale(Palette.ToBitmap(palette), 6);

                    outputBox.AppendText($"map loaded: width={map.Width}, height={map.Height}" + Environment.NewLine);
                    break;

                case "ARE":
                    AREFile area = new AREFile(entry);
                    if (area.Error != "")
                    {
                        MessageBox.Show(area.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //imgPictureBox.Image = BitmapScaler.PixelScale(, 3);
                    palettePictureBox.Image = BitmapScaler.PixelScale(Palette.ToBitmap(Palette.Default), 6);
                    break;

                case "PCC": // these are actually PCX files, version 5, encoded, 8 bits per pixel
                    PCXFile pcx = new PCXFile();
                    if (!pcx.Load(entry.Data))
                    {
                        MessageBox.Show(pcx.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    imgPictureBox.Image = BitmapScaler.PixelScale(pcx.Bitmap, 4);
                    palettePictureBox.Image = BitmapScaler.PixelScale(Palette.ToBitmap(pcx.Palette), 6);
                    break;

                default:
                    break;
            }
        }
        #endregion

        private Palette getPaletteFrom(string pccName)
        {
            var pcx = new PCXFile();
            if (pcx.Load(container.Entries[pccName].Data)) return new Palette(pcx.Palette);
            return null;
        }

        #region buttons
        private void button1_Click(object sender, EventArgs e)
        {
            loadContainer();
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (!container.Ready) return;
            container.ExportAll();
        }
        #endregion
    }
}
