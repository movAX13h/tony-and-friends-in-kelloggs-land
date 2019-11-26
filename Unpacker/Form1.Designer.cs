namespace Kelloggs
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.outputBox = new System.Windows.Forms.TextBox();
            this.exportButton = new System.Windows.Forms.Button();
            this.datFileEntriesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detailsPanel = new System.Windows.Forms.Panel();
            this.imgPictureBox = new System.Windows.Forms.PictureBox();
            this.palettePictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.paletteNameLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.detailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.palettePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // outputBox
            // 
            this.outputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputBox.Location = new System.Drawing.Point(12, 413);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(942, 71);
            this.outputBox.TabIndex = 2;
            this.outputBox.WordWrap = false;
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Location = new System.Drawing.Point(886, 365);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(68, 42);
            this.exportButton.TabIndex = 3;
            this.exportButton.Text = "SAVE ALL TO DISK";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // datFileEntriesListView
            // 
            this.datFileEntriesListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.datFileEntriesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.datFileEntriesListView.FullRowSelect = true;
            this.datFileEntriesListView.GridLines = true;
            this.datFileEntriesListView.HideSelection = false;
            this.datFileEntriesListView.Location = new System.Drawing.Point(12, 27);
            this.datFileEntriesListView.Name = "datFileEntriesListView";
            this.datFileEntriesListView.Size = new System.Drawing.Size(353, 380);
            this.datFileEntriesListView.TabIndex = 1;
            this.datFileEntriesListView.UseCompatibleStateImageBehavior = false;
            this.datFileEntriesListView.View = System.Windows.Forms.View.Details;
            this.datFileEntriesListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.datFileEntriesListView_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Filename";
            this.columnHeader1.Width = 91;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Notes";
            this.columnHeader3.Width = 140;
            // 
            // detailsPanel
            // 
            this.detailsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsPanel.AutoScroll = true;
            this.detailsPanel.BackgroundImage = global::Kelloggs.Properties.Resources.checker;
            this.detailsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.detailsPanel.Controls.Add(this.imgPictureBox);
            this.detailsPanel.Location = new System.Drawing.Point(371, 27);
            this.detailsPanel.Name = "detailsPanel";
            this.detailsPanel.Size = new System.Drawing.Size(509, 380);
            this.detailsPanel.TabIndex = 4;
            this.detailsPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.detailsPanel_Scroll);
            // 
            // imgPictureBox
            // 
            this.imgPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.imgPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgPictureBox.Location = new System.Drawing.Point(0, 0);
            this.imgPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.imgPictureBox.Name = "imgPictureBox";
            this.imgPictureBox.Size = new System.Drawing.Size(295, 217);
            this.imgPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imgPictureBox.TabIndex = 3;
            this.imgPictureBox.TabStop = false;
            // 
            // palettePictureBox
            // 
            this.palettePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.palettePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.palettePictureBox.Location = new System.Drawing.Point(886, 25);
            this.palettePictureBox.Name = "palettePictureBox";
            this.palettePictureBox.Size = new System.Drawing.Size(68, 260);
            this.palettePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.palettePictureBox.TabIndex = 4;
            this.palettePictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(371, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(509, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "- PREVIEW -";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(883, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "- PALETTE -";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // paletteNameLabel
            // 
            this.paletteNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.paletteNameLabel.ForeColor = System.Drawing.Color.Gray;
            this.paletteNameLabel.Location = new System.Drawing.Point(883, 288);
            this.paletteNameLabel.Name = "paletteNameLabel";
            this.paletteNameLabel.Size = new System.Drawing.Size(71, 13);
            this.paletteNameLabel.TabIndex = 7;
            this.paletteNameLabel.Text = "-";
            this.paletteNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(353, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "- CONTENT -";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 496);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.paletteNameLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.palettePictureBox);
            this.Controls.Add(this.detailsPanel);
            this.Controls.Add(this.datFileEntriesListView);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.detailsPanel.ResumeLayout(false);
            this.detailsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.palettePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.ListView datFileEntriesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel detailsPanel;
        private System.Windows.Forms.PictureBox imgPictureBox;
        private System.Windows.Forms.PictureBox palettePictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label paletteNameLabel;
        private System.Windows.Forms.Label label3;
    }
}

