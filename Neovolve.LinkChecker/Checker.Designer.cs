using System.Windows.Forms;

namespace Neovolve.LinkChecker
{
    partial class Checker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Pending", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Succeeded", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Failed", System.Windows.Forms.HorizontalAlignment.Left);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CheckLinks = new System.Windows.Forms.Button();
            this.ReplayChecks = new System.Windows.Forms.Button();
            this.BaseLocation = new System.Windows.Forms.ComboBox();
            this.Export = new System.Windows.Forms.Button();
            this.Import = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.FilterRegex = new System.Windows.Forms.TextBox();
            this.LinksFound = new Neovolve.LinkChecker.ListViewNF();
            this.RelativeLocation = new System.Windows.Forms.ColumnHeader();
            this.Status = new System.Windows.Forms.ColumnHeader();
            this.ResponseCode = new System.Windows.Forms.ColumnHeader();
            this.MimeType = new System.Windows.Forms.ColumnHeader();
            this.Referrer = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base Uri:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Identified links:";
            // 
            // CheckLinks
            // 
            this.CheckLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckLinks.Location = new System.Drawing.Point(1046, 736);
            this.CheckLinks.Name = "CheckLinks";
            this.CheckLinks.Size = new System.Drawing.Size(75, 23);
            this.CheckLinks.TabIndex = 3;
            this.CheckLinks.Text = "Check links";
            this.CheckLinks.UseVisualStyleBackColor = true;
            this.CheckLinks.Click += new System.EventHandler(this.CheckLinks_Click);
            // 
            // ReplayChecks
            // 
            this.ReplayChecks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplayChecks.Location = new System.Drawing.Point(965, 736);
            this.ReplayChecks.Name = "ReplayChecks";
            this.ReplayChecks.Size = new System.Drawing.Size(75, 23);
            this.ReplayChecks.TabIndex = 2;
            this.ReplayChecks.Text = "Replay checks";
            this.ReplayChecks.UseVisualStyleBackColor = true;
            this.ReplayChecks.Click += new System.EventHandler(this.ReplayChecks_Click);
            // 
            // BaseLocation
            // 
            this.BaseLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BaseLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.BaseLocation.FormattingEnabled = true;
            this.BaseLocation.Location = new System.Drawing.Point(82, 10);
            this.BaseLocation.Name = "BaseLocation";
            this.BaseLocation.Size = new System.Drawing.Size(1039, 21);
            this.BaseLocation.TabIndex = 0;
            this.BaseLocation.Text = "http://localhost/be/";
            // 
            // Export
            // 
            this.Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Export.Location = new System.Drawing.Point(884, 736);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(75, 23);
            this.Export.TabIndex = 4;
            this.Export.Text = "Export";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // Import
            // 
            this.Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Import.Location = new System.Drawing.Point(803, 736);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(75, 23);
            this.Import.TabIndex = 5;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Filter Regex:";
            // 
            // FilterRegex
            // 
            this.FilterRegex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterRegex.Location = new System.Drawing.Point(82, 37);
            this.FilterRegex.Name = "FilterRegex";
            this.FilterRegex.Size = new System.Drawing.Size(1039, 20);
            this.FilterRegex.TabIndex = 7;
            this.FilterRegex.Text = "login\\.aspx";
            // 
            // LinksFound
            // 
            this.LinksFound.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LinksFound.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RelativeLocation,
            this.Status,
            this.ResponseCode,
            this.MimeType,
            this.Referrer});
            this.LinksFound.Sorting = SortOrder.None;
            this.LinksFound.FullRowSelect = true;
            listViewGroup1.Header = "Pending";
            listViewGroup1.Name = "Pending";
            listViewGroup2.Header = "Succeeded";
            listViewGroup2.Name = "Succeeded";
            listViewGroup3.Header = "Failed";
            listViewGroup3.Name = "Failed";
            this.LinksFound.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.LinksFound.Location = new System.Drawing.Point(16, 94);
            this.LinksFound.Name = "LinksFound";
            this.LinksFound.Size = new System.Drawing.Size(1105, 636);
            this.LinksFound.TabIndex = 1;
            this.LinksFound.UseCompatibleStateImageBehavior = false;
            this.LinksFound.View = System.Windows.Forms.View.Details;
            this.LinksFound.ItemActivate += new System.EventHandler(this.LinksFound_ItemActivate);
            // 
            // RelativeLocation
            // 
            this.RelativeLocation.Text = "Relative Location";
            this.RelativeLocation.Width = 522;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 140;
            // 
            // ResponseCode
            // 
            this.ResponseCode.Text = "Response Code";
            this.ResponseCode.Width = 103;
            // 
            // MimeType
            // 
            this.MimeType.Text = "Mime Type";
            this.MimeType.Width = 274;
            // 
            // Referrer
            // 
            this.Referrer.Text = "Referrer";
            // 
            // Checker
            // 
            this.AcceptButton = this.CheckLinks;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 771);
            this.Controls.Add(this.FilterRegex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Import);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.BaseLocation);
            this.Controls.Add(this.ReplayChecks);
            this.Controls.Add(this.CheckLinks);
            this.Controls.Add(this.LinksFound);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Checker";
            this.Text = "Link Checker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ListViewNF LinksFound;
        private System.Windows.Forms.Button CheckLinks;
        private System.Windows.Forms.Button ReplayChecks;
        private System.Windows.Forms.ComboBox BaseLocation;
        private System.Windows.Forms.ColumnHeader RelativeLocation;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.ColumnHeader ResponseCode;
        private System.Windows.Forms.ColumnHeader MimeType;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FilterRegex;
        private System.Windows.Forms.ColumnHeader Referrer;
    }
}

