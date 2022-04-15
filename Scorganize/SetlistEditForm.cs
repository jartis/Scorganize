using PdfSharpCore.Pdf;
using System.Configuration;
using System.Text.Json;

namespace Scorganize
{
    public partial class SetlistEditForm : Form
    {
        private Catalog catalog;

        private bool listSaved = true;

        public SetlistEditForm(Catalog c)
        {
            InitializeComponent();
            Cursor.Current = Cursors.Default;
            this.FormClosing += SetlistEditForm_FormClosing;
            catalog = c;
            PopulateTreeView();
            SetlistTreeview.NodeMouseDoubleClick += SetlistTreeview_NodeMouseDoubleClick;
            SetListBox.DisplayMember = "Title";
            SetListBox.DoubleClick += SetListBox_DoubleClick;
            SetListBox.DragOver += SetListBox_DragOver;
            SetListBox.DragDrop += SetListBox_DragDrop;
            SetListBox.MouseDown += SetListBox_MouseDown;
            SetListBox.AllowDrop = true;
        }

        private void SetListBox_DoubleClick(object? sender, EventArgs e)
        {
            if (SetListBox.SelectedItem != null)
            {
                SetListBox.Items.Remove(SetListBox.SelectedItem);
                listSaved = false;
            }
        }

        private void SetListBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (this.SetListBox.SelectedItem == null) return;
                var si = this.SetListBox.SelectedItem;
                this.SetListBox.DoDragDrop(this.SetListBox.SelectedItem, DragDropEffects.Move);
                this.SetListBox.SelectedItem = si;
            }
        }

        private void SetListBox_DragOver(object? sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void SetListBox_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data is null) { return; }
            Point point = SetListBox.PointToClient(new Point(e.X, e.Y));
            int index = this.SetListBox.IndexFromPoint(point);
            if (index < 0) index = this.SetListBox.Items.Count - 1;
            object data = e.Data.GetData(typeof(SetlistEntry));
            this.SetListBox.Items.Remove(data);
            this.SetListBox.Items.Insert(index, data);
            listSaved = false;
        }

        private void SetlistTreeview_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                TreeTag tt = (TreeTag)e.Node.Tag;
                if (tt.tagType == TagType.Song)
                {
                    SetlistEntry s = new SetlistEntry(tt.Title, tt.Filename, tt.Page, tt.NumPages, 1);
                    SetListBox.Items.Add(s);
                    listSaved = false;
                }
            }
        }

        private void SetlistEditForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!listSaved)
            {
                var wannaSave = MessageBox.Show("Your setlist has unsaved changes! Do you want to save this setlist?", "", MessageBoxButtons.YesNoCancel);
                switch (wannaSave)
                {
                    case DialogResult.Yes:
                        SaveSetlist();
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        public void LoadSetlist()
        {
            using (OpenFileDialog? form = new OpenFileDialog())
            {
                form.Multiselect = false;
                form.Filter = "Scorganize Setlists|*.gig";
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (StreamReader r = new StreamReader(form.FileName))
                    {
                        string jsonString = r.ReadToEnd();
                        Setlist l = JsonSerializer.Deserialize<Setlist>(jsonString) ?? new Setlist();
                        foreach (SetlistEntry entry in l.Entries)
                        {
                            SetListBox.Items.Add(entry);
                        }
                    }
                }
            }
        }

        public void SaveSetlist()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Scorganizer Setlist|*.gig";
            saveFileDialog.Title = "Save Setlist File";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                try
                {
                    Setlist list = new Setlist();
                    foreach (object? sle in SetListBox.Items)
                    {
                        if (sle is null) { continue; }
                        list.Add((SetlistEntry)sle);
                    }
                    list.ResetPageNumbers();
                    string setlistString = JsonSerializer.Serialize(list);
                    using (FileStream fs = (FileStream)saveFileDialog.OpenFile())
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.Write(setlistString);
                            sw.Flush();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Error saving setlist");
                }
                listSaved = true;
            }
        }

        public void PopulateTreeView()
        {
            List<TreeNode> newNodes = new List<TreeNode>();
            this.SetlistTreeview.Nodes.Clear();
            if (catalog.BookCount > 0)
            {
                foreach (Songbook songbook in catalog.Songbooks)
                {
                    TreeNode bookNode = new TreeNode();
                    bookNode.Text = songbook.Title;
                    bookNode.Tag = new TreeTag(songbook.Title, songbook.Filename, -1, -1, TagType.Book); // Sentinel for "open book"
                    foreach (Song song in songbook.Songs)
                    {
                        TreeNode songNode = new TreeNode();
                        songNode.Tag = new TreeTag(song.Title, songbook.Filename, song.FirstPage, song.NumPages, TagType.Song);
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

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (this.SetListBox.SelectedItem == null) return;
            this.SetListBox.Items.Remove(this.SetListBox.SelectedItem);
            this.SetListBox.SelectedItem = null;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (SetlistTreeview.SelectedNode == null) return;
            TreeTag tt = (TreeTag)SetlistTreeview.SelectedNode.Tag;
            if (tt.tagType == TagType.Song)
            {
                SetlistEntry s = new SetlistEntry(tt.Title, tt.Filename, tt.Page, tt.NumPages, 1);
                SetListBox.Items.Add(s);
                listSaved = false;
            }
        }

        private void saveSetlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSetlist();
        }

        private void openSetlistMenuItem_Click(object sender, EventArgs e)
        {
            LoadSetlist();
        }

        private void playSetlistToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}