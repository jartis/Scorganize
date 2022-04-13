using PdfSharpCore.Pdf;
using System.Configuration;

namespace Scorganize
{
    public partial class SetlistEditForm : Form
    {
        private Catalog catalog;
        public SetlistEditForm()
        {
            InitializeComponent();
            Cursor.Current = Cursors.Default;
            this.FormClosing += SetlistEditForm_FormClosing;
            catalog = ((MainForm)Parent).MainCatalog;
        }

        private void SetlistEditForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void PopulateTreeView()
        {
            List<TreeNode> newNodes = new List<TreeNode>();
            this.SetlistTreeview.Nodes.Clear();
            if (catalog.BookCount > 0)
            {
                foreach (Songbook songbook in catalog)
                {
                    TreeNode bookNode = new TreeNode();
                    bookNode.Text = songbook.Title;
                    bookNode.Tag = new TreeTag(songbook.Title, songbook.Filename, -1, TagType.Book); // Sentinel for "open book"
                    foreach (Song song in songbook)
                    {
                        TreeNode songNode = new TreeNode();
                        songNode.Tag = new TreeTag(song.Title, songbook.Filename, song.FirstPage, TagType.Song);
                        songNode.Text = song.Title;
                        bookNode.Nodes.Add(songNode);
                    }
                    newNodes.Add(bookNode);
                }
                SetlistTreeview.Nodes.AddRange(newNodes.ToArray());
            }
            else
            {
                SetlistTreeview.Nodes.Add("No songbooks loaded");
            }
            SetlistTreeview.Refresh();
        }
    }
}