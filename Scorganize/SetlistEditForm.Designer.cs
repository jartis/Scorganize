using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.IO;
using System.Linq;

namespace Scorganize
{
    partial class SetlistEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetlistEditForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SetListBox = new System.Windows.Forms.ListBox();
            this.SetlistTreeview = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSetlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSetlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.SetListBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.SetlistTreeview, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 426);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // SetListBox
            // 
            this.SetListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetListBox.FormattingEnabled = true;
            this.SetListBox.ItemHeight = 15;
            this.SetListBox.Location = new System.Drawing.Point(435, 3);
            this.SetListBox.Name = "SetListBox";
            this.SetListBox.Size = new System.Drawing.Size(362, 420);
            this.SetListBox.TabIndex = 1;
            // 
            // SetlistTreeview
            // 
            this.SetlistTreeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetlistTreeview.Location = new System.Drawing.Point(3, 3);
            this.SetlistTreeview.Name = "SetlistTreeview";
            this.SetlistTreeview.Size = new System.Drawing.Size(362, 420);
            this.SetlistTreeview.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.RemoveButton, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.AddButton, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(371, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(58, 420);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // RemoveButton
            // 
            this.RemoveButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RemoveButton.Location = new System.Drawing.Point(7, 251);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(44, 23);
            this.RemoveButton.TabIndex = 4;
            this.RemoveButton.Text = "◀";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddButton.Location = new System.Drawing.Point(7, 146);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(44, 23);
            this.AddButton.TabIndex = 3;
            this.AddButton.Text = "▶";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(800, 24);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.saveSetlistToolStripMenuItem,
            this.playSetlistToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "&File";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem3.Text = "&Open Setlist";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.openSetlistMenuItem_Click);
            // 
            // saveSetlistToolStripMenuItem
            // 
            this.saveSetlistToolStripMenuItem.Name = "saveSetlistToolStripMenuItem";
            this.saveSetlistToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveSetlistToolStripMenuItem.Text = "&Save Setlist";
            this.saveSetlistToolStripMenuItem.Click += new System.EventHandler(this.saveSetlistToolStripMenuItem_Click);
            // 
            // playSetlistToolStripMenuItem
            // 
            this.playSetlistToolStripMenuItem.Name = "playSetlistToolStripMenuItem";
            this.playSetlistToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.playSetlistToolStripMenuItem.Text = "&Play Setlist";
            this.playSetlistToolStripMenuItem.Click += new System.EventHandler(this.playSetlistToolStripMenuItem_Click);
            // 
            // SetlistEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip2;
            this.Name = "SetlistEditForm";
            this.Text = "Scorganize";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem importPDFToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem setlistsToolStripMenuItem;
        private ToolStripMenuItem newSetlistToolStripMenuItem;
        private ToolStripMenuItem openSetlistToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private ListBox SetListBox;
        private TreeView SetlistTreeview;
        private TableLayoutPanel tableLayoutPanel2;
        private Button RemoveButton;
        private Button AddButton;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem saveSetlistToolStripMenuItem;
        private ToolStripMenuItem playSetlistToolStripMenuItem;
    }
}