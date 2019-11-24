using Kelloggs.Formats;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Kelloggs
{
    public partial class Form1 : Form
    {
        private string filename = "../../../Kelloggs/PCKELL.DAT";
        DATFile container;

        public Form1()
        {
            InitializeComponent();
            datFileEntriesListView.SelectedIndexChanged += datFileEntriesListView_SelectedIndexChanged;
        }

        #region buttons
        private void button1_Click(object sender, EventArgs e)
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
            foreach(DATFileEntry entry in container.Entries.Values)
            {
                var item = new ListViewItem(new string[] { entry.Filename, entry.TypeName, entry.Note });
                item.Tag = entry;
                datFileEntriesListView.Items.Add(item);
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (!container.Ready) return;

            container.ExportAll();
        }
        #endregion

        #region file list & details
        private void datFileEntriesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (datFileEntriesListView.SelectedItems.Count == 0) return;
            displayFileDetails(datFileEntriesListView.SelectedItems[0].Tag as DATFileEntry);
        }

        private void displayFileDetails(DATFileEntry entry)
        {
            detailsFileNameLabel.Text = entry.Filename;

            switch (entry.Type)
            {
                case "BOB":
                    BOBFile b = new BOBFile(entry);
                    if (b.Error != "")
                    {
                        MessageBox.Show(b.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var frames = BOBDecoder.DecodeFrames(b);
                    imgPictureBox.Image = VGABitmapConverter.ToRGBA(frames[0]);
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
