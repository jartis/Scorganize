using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.IO;
using System.Linq;

namespace Scorganize
{
    partial class PlaySetlistForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaySetlistForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SetlistListbox = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LeftBox = new System.Windows.Forms.PictureBox();
            this.RightBox = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ForwardBtn = new System.Windows.Forms.ToolStripButton();
            this.BackBtn = new System.Windows.Forms.ToolStripButton();
            this.ExportPdfButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LeftBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightBox)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SetlistListbox);
            this.splitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 191;
            this.splitContainer1.TabIndex = 0;
            // 
            // SetlistListbox
            // 
            this.SetlistListbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetlistListbox.FormattingEnabled = true;
            this.SetlistListbox.ItemHeight = 15;
            this.SetlistListbox.Location = new System.Drawing.Point(0, 0);
            this.SetlistListbox.Name = "SetlistListbox";
            this.SetlistListbox.Size = new System.Drawing.Size(187, 446);
            this.SetlistListbox.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.LeftBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.RightBox, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(601, 421);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // LeftBox
            // 
            this.LeftBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftBox.Location = new System.Drawing.Point(3, 3);
            this.LeftBox.Name = "LeftBox";
            this.LeftBox.Size = new System.Drawing.Size(294, 415);
            this.LeftBox.TabIndex = 0;
            this.LeftBox.TabStop = false;
            // 
            // RightBox
            // 
            this.RightBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightBox.Location = new System.Drawing.Point(303, 3);
            this.RightBox.Name = "RightBox";
            this.RightBox.Size = new System.Drawing.Size(295, 415);
            this.RightBox.TabIndex = 1;
            this.RightBox.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ForwardBtn,
            this.BackBtn,
            this.ExportPdfButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(601, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ForwardBtn
            // 
            this.ForwardBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ForwardBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ForwardBtn.Image = ((System.Drawing.Image)(resources.GetObject("ForwardBtn.Image")));
            this.ForwardBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ForwardBtn.Name = "ForwardBtn";
            this.ForwardBtn.Size = new System.Drawing.Size(23, 22);
            this.ForwardBtn.Text = "⏵";
            this.ForwardBtn.ToolTipText = "Forward One Page";
            this.ForwardBtn.Click += new System.EventHandler(this.ForwardBtn_Click);
            // 
            // BackBtn
            // 
            this.BackBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BackBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BackBtn.Image = ((System.Drawing.Image)(resources.GetObject("BackBtn.Image")));
            this.BackBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BackBtn.Name = "BackBtn";
            this.BackBtn.Size = new System.Drawing.Size(23, 22);
            this.BackBtn.Text = "⏴";
            this.BackBtn.ToolTipText = "Back one page";
            this.BackBtn.Click += new System.EventHandler(this.BackBtn_Click);
            // 
            // ExportPdfButton
            // 
            this.ExportPdfButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ExportPdfButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportPdfButton.Image")));
            this.ExportPdfButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportPdfButton.Name = "ExportPdfButton";
            this.ExportPdfButton.Size = new System.Drawing.Size(117, 22);
            this.ExportPdfButton.Text = "Export Setlist as PDF";
            this.ExportPdfButton.Click += new System.EventHandler(this.ExportPdfButton_Click);
            // 
            // PlaySetlistForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "PlaySetlistForm";
            this.Text = "Scorganize";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LeftBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightBox)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private ToolStrip toolStrip1;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox LeftBox;
        private PictureBox RightBox;
        private ToolStripButton ForwardBtn;
        private ToolStripButton BackBtn;
        private ListBox SetlistListbox;
        private ToolStripButton ExportPdfButton;
    }
}