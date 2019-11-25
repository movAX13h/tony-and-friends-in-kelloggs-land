using Kelloggs.Formats;
using Kelloggs.Tool;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Kelloggs
{
    public partial class Form1 : Form
    {
        private const string filename = "../../../Kelloggs/PCKELL.DAT";

        private DATFile container;
        private string baseTitle = "Kelloggs Tools"; 

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
                    
                    imgPictureBox.Image = BOBPainter.MakeSheet(b);
                    break;

                case "MAP":
                    MAPFile map = new MAPFile(entry);
                    if (map.Error != "")
                    {
                        MessageBox.Show(map.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    imgPictureBox.Image = MAPPainter.Paint(map, 3);
                    break;

                case "PCC": // these are actually PCX files, version 5, encoded, 8 bits per pixel
                    PCXFile pcx = new PCXFile();
                    if (!pcx.Load(entry.Data))
                    {
                        MessageBox.Show(pcx.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    imgPictureBox.Image = pcx.Bitmap;
                    break;

                case "ICO":
                    //TODO: better error handling
                    var icoImages = ICOPainter.ICOToBitmaps(entry.Data, Palette.Default);
                    if (icoImages.Length == 0)
                    {
                        MessageBox.Show("Failed to load ICO, sorry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    imgPictureBox.Image = ICOPainter.TileSetFromBitmaps(icoImages);

                    break;

                default:
                    break;
            }
        }
        #endregion

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
