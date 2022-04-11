using PdfSharpCore.Pdf;

namespace Scorganize
{
    public partial class MainForm : Form
    {
        public Catalog MainCatalog { get; set; }

        private string CatFile = @"D:\scorgcat.dat";
        private List<TreeNode> nodeCache;

        public delegate void SetStatusDelegate(string str, int pct);
        public SetStatusDelegate SetStatus;
        private Random random;

        private int curPage = 1;

        public MainForm()
        {
            InitializeComponent();
            SetStatus = new SetStatusDelegate(_setStatus);
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(MainForm_DragEnter);
            this.DragDrop += new DragEventHandler(MainForm_DragDrop);
            this.CatalogTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(MainForm_TreeNodeClicked);
            this.CatalogTreeView.NodeMouseClick += (sender, args) => this.CatalogTreeView.SelectedNode = args.Node;
            this.SearchBox.TextChanged += SearchBox_TextChanged;
            Cursor.Current = Cursors.Default;
            MainCatalog = new Catalog();
            random = new Random();
        }

        private void PopContextMenu(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeTag tag = (TreeTag)e.Node.Tag;
            ContextMenuStrip menuStrip = new ContextMenuStrip();
            if (tag.tagType == TagType.Book)
            {
                makeBookContextMenu(e, tag, menuStrip);
            }
            else if (tag.tagType == TagType.Song)
            {
                makeSongContextMenu(e, tag, menuStrip);
            }
        }

        private void makeBookContextMenu(TreeNodeMouseClickEventArgs e, TreeTag tag, ContextMenuStrip menuStrip)
        {
            ToolStripMenuItem editItem = new ToolStripMenuItem("Edit book information...");
            editItem.Click += (sender, args) =>
            {
                EditBookDialog editBookDialog = new EditBookDialog(tag.Title, tag.Filename);
                DialogResult result = editBookDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    Songbook book = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                    book.Title = editBookDialog.BookName;
                    book.Filename = editBookDialog.FileName;
                    MainCatalog.Save(CatFile);
                    PopulateTreeView();
                }
                editBookDialog.Dispose();
            };
            ToolStripMenuItem shiftBackItem = new ToolStripMenuItem("Shift page numbers back one");
            shiftBackItem.Click += (sender, args) =>
            {
                Songbook book = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                foreach (Song s in book.Songs)
                {
                    s.Page = s.Page - 1;
                }
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            };
            ToolStripMenuItem shiftForwardItem = new ToolStripMenuItem("Shift page numbers forward one");
            shiftForwardItem.Click += (sender, args) =>
            {
                Songbook book = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                foreach (Song s in book.Songs)
                {
                    s.Page = s.Page + 1;
                }
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            };
            ToolStripMenuItem deleteSongsItem = new ToolStripMenuItem("Remove all song bookmarks");
            deleteSongsItem.Click += (sender, args) =>
            {
                DialogResult confirmResult = MessageBox.Show("Are you sure want to remove all bookmarks from this book?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    Songbook book = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                    MainCatalog.Songbooks.Clear();
                    MainCatalog.Save(CatFile);
                    PopulateTreeView();
                }
            };
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Remove Songbook");
            deleteItem.Click += (sender, args) =>
            {
                DialogResult confirmResult = MessageBox.Show("Are you sure want to remove this songbook?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    Songbook book = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                    MainCatalog.Songbooks.Remove(book);
                    MainCatalog.Save(CatFile);
                    PopulateTreeView();
                }
            };

            menuStrip.Items.Clear();
            menuStrip.Items.Add(editItem);
            menuStrip.Items.Add(shiftBackItem);
            menuStrip.Items.Add(shiftForwardItem);
            menuStrip.Items.Add(new ToolStripSeparator());
            menuStrip.Items.Add(deleteItem);
            menuStrip.Show(this, e.Location);
        }

        private void makeSongContextMenu(TreeNodeMouseClickEventArgs e, TreeTag tag, ContextMenuStrip menuStrip)
        {
            ToolStripMenuItem editItem = new ToolStripMenuItem("Edit song information...");
            editItem.Click += (sender, args) =>
            {
                Songbook book = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                Song song = book.Songs.First(s => s.Title == tag.Title);

                EditSongDialog editSongDialog = new EditSongDialog(song.Title, song.Artist, song.Page);
                DialogResult result = editSongDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    song.Title = editSongDialog.SongTitle;
                    song.Artist = editSongDialog.SongArtist;
                    song.Page = editSongDialog.SongPage;

                    MainCatalog.Save(CatFile);
                    PopulateTreeView();
                }
                editSongDialog.Dispose();
            };
            ToolStripMenuItem shiftBackItem = new ToolStripMenuItem("Shift page number back one");
            shiftBackItem.Click += (sender, args) =>
            {
                Songbook book = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                Song song = book.Songs.First(s => s.Title == tag.Title);
                song.Page = song.Page - 1;
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            };
            ToolStripMenuItem shiftForwardItem = new ToolStripMenuItem("Shift page number forward one");
            shiftForwardItem.Click += (sender, args) =>
            {
                Songbook book = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                Song song = book.Songs.First(s => s.Title == tag.Title);
                song.Page = song.Page + 1;
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            };
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Remove Song Bookmark");
            deleteItem.Click += (sender, args) =>
            {
                DialogResult confirmResult = MessageBox.Show("Are you sure want to remove this song bookmark?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    Songbook book = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                    Song song = book.Songs.First(s => s.Title == tag.Title);
                    book.Songs.Remove(song);
                    MainCatalog.Save(CatFile);
                    PopulateTreeView();
                }
            };
            menuStrip.Items.Clear();
            menuStrip.Items.Add(editItem);
            menuStrip.Items.Add(shiftBackItem);
            menuStrip.Items.Add(shiftForwardItem);
            menuStrip.Items.Add(new ToolStripSeparator());
            menuStrip.Items.Add(deleteItem);
            menuStrip.Show(this, e.Location);
        }

        public void _setStatus(string str, int pct)
        {
            StatusLabel.Text = str;
            ProcessProgressBar.Value = pct;
            PopulateTreeView();
        }

        private void SearchBox_TextChanged(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBox.Text))
            {
                //blocks repainting tree till all objects loaded
                CatalogTreeView.BeginUpdate();
                CatalogTreeView.Nodes.Clear();

                foreach (TreeNode _parentNode in nodeCache)
                {
                    foreach (TreeNode _childNode in _parentNode.Nodes)
                    {
                        if (_childNode.Text.ToLower().Contains(SearchBox.Text.ToLower()))
                        {
                            this.CatalogTreeView.Nodes.Add((TreeNode)_childNode.Clone());
                        }
                    }
                }
                //enables redrawing tree after all objects have been added
                this.CatalogTreeView.EndUpdate();
            }
            else
            {
                PopulateTreeView();
            }
        }

        void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        public async void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            Thread t = new Thread(() => HandleDrag(files, this));
            t.Start();
        }

        private void HandleDrag(string[] files, MainForm f)
        {
            int position = 0;
            foreach (string file in files)
            {
                if (MainCatalog.Songbooks.Any(b => b.Filename == file))
                {
                    Interlocked.Increment(ref position);
                    f.Invoke(f.SetStatus, new Object[] { $"{Path.GetFileName(file)} already exists", (int)(100 * position / files.Length) });
                    continue;
                }
                Task task = Task.Factory.StartNew(() =>
                {
                    Songbook book;
                    book = Songbook.FromFile(file);
                    if (book != null)
                    {
                        MainCatalog.Songbooks.Add(book);
                    }
                });

                if (task.Wait(60000)) // Specify overall timeout for Process() here
                {
                    Interlocked.Increment(ref position);
                    f.Invoke(f.SetStatus, new Object[] { $"Processed {Path.GetFileName(file)}", (int)(100 * position / files.Length) });
                }
                else
                {
                    Interlocked.Increment(ref position);
                    f.Invoke(f.SetStatus, new Object[] { $"Error processing {Path.GetFileName(file)}", (int)(100 * position / files.Length) });
                }

            }
            MainCatalog.Save(CatFile);
            f.Invoke(() =>
            {
                StatusLabel.Text = $"Ready";
                ProcessProgressBar.Value = 0;
                PopulateTreeView();
            });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MainCatalog = Catalog.Load(CatFile);
            nodeCache = new List<TreeNode>();
            PopulateTreeView();
        }

        private void MainForm_TreeNodeClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeTag tag = (TreeTag)e.Node.Tag;
            if (e.Button == MouseButtons.Left)
            {
                if (e.Node.Tag != null)
                {
                    PdfiumViewer.PdfDocument curDoc = PdfiumViewer.PdfDocument.Load(tag.Filename);
                    curPage = 1;
                    if (tag.Page > -1)
                    {
                        curPage = tag.Page;
                    }

                    LeftPage.Load(curDoc);
                    RightPage.Load(curDoc);
                    LeftPage.Page = curPage;
                    RightPage.Page = curPage + 1;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                PopContextMenu(sender, e);
            }
        }



        public void PopulateTreeView()
        {
            List<TreeNode> newNodes = new List<TreeNode>();
            this.CatalogTreeView.Nodes.Clear();
            foreach (Songbook songbook in MainCatalog.Songbooks)
            {
                TreeNode bookNode = new TreeNode();
                bookNode.Text = songbook.Title;
                bookNode.Tag = new TreeTag(songbook.Title, songbook.Filename, -1, TagType.Book); // Sentinel for "open book"
                foreach (Song song in songbook.Songs)
                {
                    TreeNode songNode = new TreeNode();
                    songNode.Tag = new TreeTag(song.Title, songbook.Filename, song.Page, TagType.Song);
                    songNode.Text = song.Title;
                    bookNode.Nodes.Add(songNode);
                }
                newNodes.Add(bookNode);
            }
            this.CatalogTreeView.Nodes.AddRange(newNodes.ToArray());
            nodeCache.Clear();
            foreach (TreeNode node in this.CatalogTreeView.Nodes)
            {
                nodeCache.Add((TreeNode)node.Clone());
            }
            this.CatalogTreeView.Refresh();
        }

        private void RandomSongButton_Click(object sender, EventArgs e)
        {
            List<TreeNode> songNodes = new List<TreeNode>();
            foreach (TreeNode node in nodeCache)
            {
                if (node.Nodes.Count > 0)
                {
                    foreach (TreeNode t in node.Nodes)
                    {
                        songNodes.Add(t);
                    }
                }
            }
            if (songNodes.Count > 0)
            {
                MainForm_TreeNodeClicked(sender, new TreeNodeMouseClickEventArgs(songNodes[random.Next(songNodes.Count)], MouseButtons.Left, 1, 0, 0));
            }
        }
    }
}