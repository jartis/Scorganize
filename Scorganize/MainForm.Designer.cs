using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.IO;
using System.Linq;

namespace Scorganize
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SearchBox = new Scorganize.ClearableTextBox();
            this.CatalogTreeView = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LeftBox = new System.Windows.Forms.PictureBox();
            this.RightBox = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.RandomSongButton = new System.Windows.Forms.ToolStripButton();
            this.AddSongButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveSongButton = new System.Windows.Forms.ToolStripButton();
            this.ForwardBtn = new System.Windows.Forms.ToolStripButton();
            this.BackBtn = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProcessProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LeftBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightBox)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SearchBox);
            this.splitContainer1.Panel1.Controls.Add(this.CatalogTreeView);
            this.splitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.splitContainer1.Size = new System.Drawing.Size(800, 426);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // SearchBox
            // 
            this.SearchBox.ButtonTextClear = true;
            this.SearchBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SearchBox.Location = new System.Drawing.Point(0, 399);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(262, 23);
            this.SearchBox.TabIndex = 1;
            // 
            // CatalogTreeView
            // 
            this.CatalogTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CatalogTreeView.Location = new System.Drawing.Point(0, 0);
            this.CatalogTreeView.Name = "CatalogTreeView";
            this.CatalogTreeView.Size = new System.Drawing.Size(262, 422);
            this.CatalogTreeView.TabIndex = 0;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(526, 375);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // LeftBox
            // 
            this.LeftBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftBox.Location = new System.Drawing.Point(3, 3);
            this.LeftBox.Name = "LeftBox";
            this.LeftBox.Size = new System.Drawing.Size(257, 369);
            this.LeftBox.TabIndex = 0;
            this.LeftBox.TabStop = false;
            // 
            // RightBox
            // 
            this.RightBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightBox.Location = new System.Drawing.Point(266, 3);
            this.RightBox.Name = "RightBox";
            this.RightBox.Size = new System.Drawing.Size(257, 369);
            this.RightBox.TabIndex = 1;
            this.RightBox.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RandomSongButton,
            this.AddSongButton,
            this.RemoveSongButton,
            this.ForwardBtn,
            this.BackBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(526, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // RandomSongButton
            // 
            this.RandomSongButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.RandomSongButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RandomSongButton.Image = ((System.Drawing.Image)(resources.GetObject("RandomSongButton.Image")));
            this.RandomSongButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RandomSongButton.Name = "RandomSongButton";
            this.RandomSongButton.Size = new System.Drawing.Size(100, 22);
            this.RandomSongButton.Text = "🎲 Random Song";
            this.RandomSongButton.Click += new System.EventHandler(this.RandomSongButton_Click);
            // 
            // AddSongButton
            // 
            this.AddSongButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AddSongButton.Image = ((System.Drawing.Image)(resources.GetObject("AddSongButton.Image")));
            this.AddSongButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddSongButton.Name = "AddSongButton";
            this.AddSongButton.Size = new System.Drawing.Size(135, 22);
            this.AddSongButton.Text = "➕ Add Song Bookmark";
            this.AddSongButton.Visible = false;
            this.AddSongButton.Click += new System.EventHandler(this.AddSongButton_Click);
            // 
            // RemoveSongButton
            // 
            this.RemoveSongButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RemoveSongButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveSongButton.Image")));
            this.RemoveSongButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveSongButton.Name = "RemoveSongButton";
            this.RemoveSongButton.Size = new System.Drawing.Size(156, 22);
            this.RemoveSongButton.Text = "🚫 Remove Song Bookmark";
            this.RemoveSongButton.Visible = false;
            this.RemoveSongButton.Click += new System.EventHandler(this.RemoveSongButton_Click);
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProcessProgressBar,
            this.StatusLabel});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 400);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(526, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProcessProgressBar
            // 
            this.ProcessProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ProcessProgressBar.Name = "ProcessProgressBar";
            this.ProcessProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(39, 17);
            this.StatusLabel.Spring = true;
            this.StatusLabel.Text = "Ready";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importPDFToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // importPDFToolStripMenuItem
            // 
            this.importPDFToolStripMenuItem.Name = "importPDFToolStripMenuItem";
            this.importPDFToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.importPDFToolStripMenuItem.Text = "Imp&ort PDF...";
            this.importPDFToolStripMenuItem.Click += new System.EventHandler(this.importPDFToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(140, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Scorganize";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LeftBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightBox)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SplitContainer splitContainer1;
        private TreeView CatalogTreeView;
        private ClearableTextBox SearchBox;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar ProcessProgressBar;
        private ToolStripStatusLabel StatusLabel;
        private ToolStrip toolStrip1;
        private ToolStripButton RandomSongButton;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox LeftBox;
        private PictureBox RightBox;
        private ToolStripButton AddSongButton;
        private ToolStripButton RemoveSongButton;
        private ToolStripButton ForwardBtn;
        private ToolStripButton BackBtn;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem importPDFToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
    }
}