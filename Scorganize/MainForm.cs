using PdfSharpCore.Pdf;
using System.Configuration;

namespace Scorganize
{
    public partial class MainForm : Form
    {
        public Catalog MainCatalog { get; set; }

        private string CatFile = "";
        private List<TreeNode> nodeCache;

        public delegate void SetStatusDelegate(string str, int pct);
        public SetStatusDelegate SetStatus;
        private Random random;
        private PdfiumViewer.PdfRenderer pdfRenderer;
        private Graphics leftG;
        private Graphics rightG;
        private PdfiumViewer.PdfDocument curDoc;
        private int curPage = 1;
        private Songbook curBook;
        private AboutBox abt;

        private void GetConfig()
        {
            if (Program.Conf.AppSettings.Settings.AllKeys.Contains("catfilepath"))
            {
                CatFile = Program.Conf.AppSettings.Settings["catfilepath"].Value;
            }
            else
            {
                CatFile = Path.Combine(Application.UserAppDataPath, "scorgcat.dat");
                Program.Conf.AppSettings.Settings.Add("catfilepath", CatFile);
                Program.Conf.Save();
            }
        }

        public MainForm()
        {
            InitializeComponent();

            GetConfig();



            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
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
            pdfRenderer = new PdfiumViewer.PdfRenderer();
            leftG = LeftBox.CreateGraphics();
            rightG = RightBox.CreateGraphics();
            this.splitContainer1.SplitterMoved += new SplitterEventHandler((sender, e) => this.Invalidate());
            this.PreviewKeyDown += new PreviewKeyDownEventHandler(MainForm_PreviewKeyDown);
            CatalogTreeView.KeyDown += CatalogTreeView_KeyDown;
            this.MouseWheel += MainForm_MouseWheel;
        }

        private void MainForm_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                PageForward();
            }
            else if (e.Delta > 0)
            {
                PageBack();
            }
        }

        private void CatalogTreeView_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.PageUp:
                case Keys.PageDown:
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private void MainForm_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                case Keys.PageDown:
                case Keys.PageUp:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            int pageChange = 0;
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.PageUp:
                    PageBack();
                    break;
                case Keys.Right:
                case Keys.Down:
                case Keys.PageDown:
                    PageForward();
                    break;
            }
        }

        private void PageForward()
        {
            if (curDoc != null)
            {
                curPage = Math.Min(curPage + 1, curDoc.PageCount - 1);
                SetSongButtons();
                Invalidate();
            }
        }

        private void PageBack()
        {
            if (curDoc != null)
            {
                curPage = Math.Max(curPage - 1, 1);
                SetSongButtons();
                Invalidate();
            }
        }

        private void SetSongButtons()
        {
            if (curBook.Songs.Any(s => s.Page == curPage))
            {
                RemoveSongButton.Visible = true;
                AddSongButton.Visible = false;
            }
            else
            {
                RemoveSongButton.Visible = false;
                AddSongButton.Visible = true;
            }
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
                    book.Songs.Clear();
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
            menuStrip.Items.Add(deleteSongsItem);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (curDoc != null)
            {
                LeftBox.Image = curDoc.Render(curPage, LeftBox.Width, LeftBox.Height, leftG.DpiX, leftG.DpiY, false);
                RightBox.Image = curDoc.Render(curPage + 1, RightBox.Width, RightBox.Height, rightG.DpiX, rightG.DpiY, false);
            }
            base.OnPaint(e);
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
                    if (curDoc != null)
                    {
                        curDoc.Dispose();
                    }
                    curBook = MainCatalog.Songbooks.First(b => b.Filename == tag.Filename);
                    curDoc = PdfiumViewer.PdfDocument.Load(tag.Filename);
                    curPage = 1;
                    if (tag.Page > -1)
                    {
                        curPage = tag.Page;
                    }
                    SetSongButtons();
                    this.Invalidate();
                }
                this.Activate();
                ForwardBtn.Select();
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
            if (MainCatalog.Songbooks.Count > 0)
            {
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
            }
            else
            {
                CatalogTreeView.Nodes.Add("No songbooks loaded");
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

        private void ForwardBtn_Click(object sender, EventArgs e)
        {
            PageForward();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            PageBack();
        }

        private void RemoveSongButton_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Are you sure want to remove this song bookmark?", "", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                curBook.Songs.Remove(curBook.Songs.First(s => s.Page == curPage));
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            }
        }

        private void AddSongButton_Click(object sender, EventArgs e)
        {
            Song song = new Song(curPage);

            EditSongDialog editSongDialog = new EditSongDialog("", "", curPage);
            DialogResult result = editSongDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                song.Title = editSongDialog.SongTitle;
                song.Artist = editSongDialog.SongArtist;
                song.Page = editSongDialog.SongPage;

                curBook.Songs.Add(song);
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            }
            editSongDialog.Dispose();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (abt == null)
            {
                abt = new AboutBox();
            }
            abt.ShowDialog(this);
        }

        private void importPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog? form = new OpenFileDialog())
            {
                form.Multiselect = true;
                form.Filter = "PDF Files|*.pdf";
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    HandleDrag(form.FileNames, this);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainCatalog.Save(CatFile);
            Application.Exit();
        }
    }
}