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
            this.button1 = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.exportButton = new System.Windows.Forms.Button();
            this.datFileEntriesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detailsPanel = new System.Windows.Forms.Panel();
            this.palettePictureBox = new System.Windows.Forms.PictureBox();
            this.imgPictureBox = new System.Windows.Forms.PictureBox();
            this.detailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.palettePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 26);
            this.button1.TabIndex = 0;
            this.button1.Text = "LOAD";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // outputBox
            // 
            this.outputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputBox.Location = new System.Drawing.Point(12, 413);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(942, 71);
            this.outputBox.TabIndex = 1;
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Location = new System.Drawing.Point(801, 12);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(153, 26);
            this.exportButton.TabIndex = 2;
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
            this.datFileEntriesListView.Location = new System.Drawing.Point(12, 44);
            this.datFileEntriesListView.Name = "datFileEntriesListView";
            this.datFileEntriesListView.Size = new System.Drawing.Size(353, 363);
            this.datFileEntriesListView.TabIndex = 3;
            this.datFileEntriesListView.UseCompatibleStateImageBehavior = false;
            this.datFileEntriesListView.View = System.Windows.Forms.View.Details;
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
            this.detailsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.detailsPanel.Controls.Add(this.palettePictureBox);
            this.detailsPanel.Controls.Add(this.imgPictureBox);
            this.detailsPanel.Location = new System.Drawing.Point(371, 44);
            this.detailsPanel.Name = "detailsPanel";
            this.detailsPanel.Size = new System.Drawing.Size(583, 363);
            this.detailsPanel.TabIndex = 4;
            // 
            // palettePictureBox
            // 
            this.palettePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.palettePictureBox.Location = new System.Drawing.Point(3, 3);
            this.palettePictureBox.Name = "palettePictureBox";
            this.palettePictureBox.Size = new System.Drawing.Size(50, 200);
            this.palettePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.palettePictureBox.TabIndex = 4;
            this.palettePictureBox.TabStop = false;
            // 
            // imgPictureBox
            // 
            this.imgPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgPictureBox.Location = new System.Drawing.Point(59, 3);
            this.imgPictureBox.Name = "imgPictureBox";
            this.imgPictureBox.Size = new System.Drawing.Size(295, 217);
            this.imgPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imgPictureBox.TabIndex = 3;
            this.imgPictureBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 496);
            this.Controls.Add(this.detailsPanel);
            this.Controls.Add(this.datFileEntriesListView);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.detailsPanel.ResumeLayout(false);
            this.detailsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.palettePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.ListView datFileEntriesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel detailsPanel;
        private System.Windows.Forms.PictureBox imgPictureBox;
        private System.Windows.Forms.PictureBox palettePictureBox;
    }
}

