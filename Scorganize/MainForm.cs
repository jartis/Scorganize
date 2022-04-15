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
        private Graphics leftG;
        private Graphics rightG;
        private PdfiumViewer.PdfDocument? curDoc;
        private int _curPage = 1;
        private int curPage
        {
            get
            {
                return _curPage;
            }
            set
            {
                PageNumberBox.Text = value.ToString();
                _curPage = value;
            }
        }
        private Songbook? curBook;

        private void GetConfig()
        {
            CatFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "scorgcat.dat");
        }

        public MainForm()
        {
            InitializeComponent();

            GetConfig();

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            SetStatus = new SetStatusDelegate(_setStatus);
            this.AllowDrop = true;
            this.DragEnter += MainForm_DragEnter;
            this.DragDrop += MainForm_DragDrop;
            this.CatalogTreeView.NodeMouseClick += MainForm_TreeNodeClicked;
            this.CatalogTreeView.NodeMouseClick += (sender, args) => this.CatalogTreeView.SelectedNode = args.Node;
            this.SearchBox.TextChanged += SearchBox_TextChanged;
            Cursor.Current = Cursors.Default;
            MainCatalog = new Catalog();
            random = new Random();
            leftG = LeftBox.CreateGraphics();
            rightG = RightBox.CreateGraphics();
            this.splitContainer1.SplitterMoved += new SplitterEventHandler((sender, e) => this.Invalidate());
            this.PreviewKeyDown += new PreviewKeyDownEventHandler(MainForm_PreviewKeyDown);
            CatalogTreeView.KeyDown += CatalogTreeView_KeyDown;
            this.MouseWheel += MainForm_MouseWheel;
            this.FormClosing += MainForm_FormClosing;
            PageNumberBox.TextChanged += PageNumberBox_TextChanged;
            nodeCache = new List<TreeNode>();
        }

        private void PageNumberBox_TextChanged(object? sender, EventArgs e)
        {
            if (curDoc == null) return;
            if (!(int.TryParse(PageNumberBox.Text, out int newPage)))
            {
                PageNumberBox.Text = curPage.ToString();
            }
            curPage = Math.Max(1, Math.Min(newPage, curDoc.PageCount - 1));
            Invalidate();
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            MainCatalog.Save(CatFile);
        }

        private void MainForm_MouseWheel(object? sender, MouseEventArgs e)
        {
            changePage(-Math.Sign(e.Delta));
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
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.PageUp:
                    changePage(-1);
                    break;
                case Keys.Right:
                case Keys.Down:
                case Keys.PageDown:
                    changePage(1);
                    break;
            }
        }

        private void changePage(int change)
        {
            if (curDoc is null || curBook is null) { return; }
            curPage += change;
            curPage = Math.Max(Math.Min(curPage, curDoc.PageCount - 1), 1);
            PageNumberBox.Text = curPage.ToString();
        }

        private void SetSongButtons()
        {
            if (curBook == null)
            {
                RemoveSongButton.Visible = false;
                AddSongButton.Visible = false;
            }
            else if (curBook.HasMarkerAt(curPage))
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
            if (e.Node.Tag == null)
            {
                return;
            }
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
                using (EditBookDialog editBookDialog = new EditBookDialog(tag.Title, tag.Filename))
                {
                    editBookDialog.StartPosition = FormStartPosition.CenterParent;
                    DialogResult result = editBookDialog.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        Songbook book = MainCatalog.BookFromFilename(tag.Filename);
                        book.Title = editBookDialog.BookName;
                        book.Filename = editBookDialog.FileName;
                        MainCatalog.Save(CatFile);
                        PopulateTreeView();
                    }
                }
            };
            ToolStripMenuItem shiftBackItem = new ToolStripMenuItem("Shift page numbers back one");
            shiftBackItem.Click += (sender, args) =>
            {
                MainCatalog.BookFromFilename(tag.Filename).AdjustPageNumbers(-1);
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            };
            ToolStripMenuItem shiftForwardItem = new ToolStripMenuItem("Shift page numbers forward one");
            shiftForwardItem.Click += (sender, args) =>
            {
                MainCatalog.BookFromFilename(tag.Filename).AdjustPageNumbers(1);
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            };
            ToolStripMenuItem deleteSongsItem = new ToolStripMenuItem("Remove all song bookmarks");
            deleteSongsItem.Click += (sender, args) =>
            {
                DialogResult confirmResult = MessageBox.Show("Are you sure want to remove all bookmarks from this book?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    MainCatalog.BookFromFilename(tag.Filename).Clear();
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
                    if (MainCatalog.BookFromFilename(tag.Filename) == curBook)
                    {
                        if (curDoc != null)
                        {
                            curDoc.Dispose();
                            curDoc = null;
                        }
                        SetSongButtons();
                        Invalidate();
                    }
                    MainCatalog.Remove(MainCatalog.BookFromFilename(tag.Filename));
                    MainCatalog.Save(CatFile);
                    PopulateTreeView();
                }
            };
            ToolStripMenuItem saveBookmarksItem = new ToolStripMenuItem("Save bookmarks to PDF");
            saveBookmarksItem.Click += (sender, args) =>
            {
                DialogResult confirmResult = MessageBox.Show("Are you sure you wish to replace the bookmarks in the PDF?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    if (curDoc != null)
                    {
                        curDoc.Dispose();
                    }
                    Songbook book = MainCatalog.BookFromFilename(tag.Filename);
                    if (book.ReplaceBookmarksInFile())
                    {
                        MessageBox.Show("File updated successfully", "", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Error saving the file", "", MessageBoxButtons.OK);
                    }
                }
            };

            menuStrip.Items.Clear();
            menuStrip.Items.Add(editItem);
            menuStrip.Items.Add(shiftBackItem);
            menuStrip.Items.Add(shiftForwardItem);
            menuStrip.Items.Add(new ToolStripSeparator());
            menuStrip.Items.Add(saveBookmarksItem);
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
                Songbook book = MainCatalog.BookFromFilename(tag.Filename);
                Song song = book.SongFromTitle(tag.Title);

                using (EditSongDialog editSongDialog = new EditSongDialog(song.Title, song.Artist, song.FirstPage, song.NumPages))
                {
                    editSongDialog.StartPosition = FormStartPosition.CenterParent;
                    DialogResult result = editSongDialog.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        song.Title = editSongDialog.SongTitle;
                        song.Artist = editSongDialog.SongArtist;
                        song.FirstPage = editSongDialog.SongPage;

                        MainCatalog.Save(CatFile);
                        PopulateTreeView();
                    }
                }
            };
            ToolStripMenuItem shiftBackItem = new ToolStripMenuItem("Shift page number back one");
            shiftBackItem.Click += (sender, args) =>
            {
                MainCatalog.BookFromFilename(tag.Filename).SongFromTitle(tag.Title).FirstPage -= 1;
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            };
            ToolStripMenuItem shiftForwardItem = new ToolStripMenuItem("Shift page number forward one");
            shiftForwardItem.Click += (sender, args) =>
            {
                MainCatalog.BookFromFilename(tag.Filename).SongFromTitle(tag.Title).FirstPage += 1;
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            };
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Remove Song Bookmark");
            deleteItem.Click += (sender, args) =>
            {
                DialogResult confirmResult = MessageBox.Show("Are you sure want to remove this song bookmark?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    Songbook book = MainCatalog.BookFromFilename(tag.Filename);
                    book.Remove(tag.Title);
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
            ProcessProgressBar.Style = ProgressBarStyle.Marquee;
            ProcessProgressBar.Value = 100;
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

        public void MainForm_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data is null) { return; }
            Thread t = new Thread(() =>
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string path in files)
                {
                    // get the file attributes for file or directory
                    FileAttributes attr = File.GetAttributes(path);

                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        ProcessDirectory(path);
                    }
                    else
                    {
                        ProcessSingleFile(path, this);
                    }
                }
            });
            t.Start();
        }

        private void HandleDrag(string[] paths, MainForm f)
        {
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
            if (curBook != null)
            {
                if (curDoc == null)
                {
                    curDoc = PdfiumViewer.PdfDocument.Load(curBook.Filename);
                }
                LeftBox.Image = curDoc.Render(Math.Max(curPage - 1, 0), LeftBox.Width, LeftBox.Height, leftG.DpiX, leftG.DpiY, false);
                RightBox.Image = curDoc.Render(Math.Min(curPage, curDoc.PageCount), RightBox.Width, RightBox.Height, rightG.DpiX, rightG.DpiY, false);
            }
            base.OnPaint(e);
        }

        private void MainForm_TreeNodeClicked(object? sender, TreeNodeMouseClickEventArgs e)
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
                    curBook = MainCatalog.BookFromFilename(tag.Filename);
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
                if (sender is null) { return; }
                PopContextMenu(sender, e);
            }
        }

        public void PopulateTreeView()
        {
            List<TreeNode> newNodes = new List<TreeNode>();
            this.CatalogTreeView.Nodes.Clear();
            if (MainCatalog.BookCount > 0)
            {
                foreach (Songbook songbook in MainCatalog.Songbooks)
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
                if (node.Tag == null)
                {
                    continue;
                }
                TreeTag tt = (TreeTag)node.Tag;
                if (tt.tagType == TagType.Book)
                {
                    if (node.Nodes.Count > 0)
                    {
                        foreach (TreeNode t in node.Nodes)
                        {
                            songNodes.Add(t);
                        }
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
            changePage(1);
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            changePage(-1);
        }

        private void RemoveSongButton_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Are you sure want to remove this song bookmark?", "", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                curBook?.Remove(curBook.SongFromPage(curPage));
                MainCatalog.Save(CatFile);
                PopulateTreeView();
            }
        }

        private void AddSongButton_Click(object sender, EventArgs e)
        {
            Song song = new Song(curPage, 1);

            using (EditSongDialog editSongDialog = new EditSongDialog("", "", curPage, 1))
            {
                editSongDialog.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = editSongDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    song.Title = editSongDialog.SongTitle;
                    song.Artist = editSongDialog.SongArtist;
                    song.FirstPage = editSongDialog.SongPage;
                    song.NumPages = editSongDialog.NumPages;

                    curBook?.Add(song);
                    MainCatalog.Save(CatFile);
                    PopulateTreeView();
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox abt = new AboutBox())
            {
                abt.ShowDialog(this);
            }
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

        public void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessSingleFile(fileName, this);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public void ProcessSingleFile(string file, MainForm f)
        {
            if (!File.Exists(file)) { return; }
            if (Path.GetExtension(file).ToLower() != ".pdf") { return; }

            if (MainCatalog.HasFile(file))
            {
                f.Invoke(f.SetStatus, new Object[] { $"{Path.GetFileName(file)} already exists", 100 });
            }
            Task task = Task.Factory.StartNew(() =>
            {
                Songbook? book;
                book = Songbook.FromFile(file);
                if (book != null)
                {
                    MainCatalog.Add(book);
                }
            });

            if (task.Wait(60000)) // Specify overall timeout for Process() here
            {
                f.Invoke(f.SetStatus, new Object[] { $"Processed {Path.GetFileName(file)}", 100 });
            }
            else
            {
                f.Invoke(f.SetStatus, new Object[] { $"Error processing {Path.GetFileName(file)}", 100 });
            }
        }

        #region EventHandlers

        private void playSetlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PlaySetlistForm psf = new PlaySetlistForm())
            {
                psf.StartPosition = FormStartPosition.CenterParent;
                psf.LoadSetlistAndBuildDoc();
                psf.ShowDialog(this);
            }
        }

        private void newSetlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SetlistEditForm setlistEditForm = new SetlistEditForm(MainCatalog))
            {
                setlistEditForm.StartPosition = FormStartPosition.CenterParent;
                setlistEditForm.ShowDialog(this);
            }
        }

        private void openSetlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SetlistEditForm setlistEditForm = new SetlistEditForm(MainCatalog))
            {
                setlistEditForm.StartPosition = FormStartPosition.CenterParent;
                setlistEditForm.LoadSetlist();
                setlistEditForm.ShowDialog(this);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MainCatalog = Catalog.Load(CatFile);
            nodeCache = new List<TreeNode>();
            PopulateTreeView();
        }

        void MainForm_DragEnter(object? sender, DragEventArgs e)
        {
            if (e is null || e.Data is null) { return; }
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void deleteEntireCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will clear your entire catalog! Are you sure?", "Hold up...", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MainCatalog = new Catalog();
                MainCatalog.Save(CatFile);
                PopulateTreeView();
                MessageBox.Show("Catalog cleared. Godspeed.");
            }
        }

        #endregion
    }
}