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

        private int curPage = 1;

        public MainForm()
        {
            InitializeComponent();
            SetStatus = new SetStatusDelegate(_setStatus);
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(MainForm_DragEnter);
            this.DragDrop += new DragEventHandler(MainForm_DragDrop);
            this.CatalogTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(MainForm_TreeNodeClicked);
            this.SearchBox.TextChanged += SearchBox_TextChanged;
            Cursor.Current = Cursors.Default;
            MainCatalog = new Catalog();
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
            if (e.Node.Tag != null)
            {
                TreeTag tag = (TreeTag)e.Node.Tag;
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

        public void PopulateTreeView()
        {
            List<TreeNode> newNodes = new List<TreeNode>();
            this.CatalogTreeView.Nodes.Clear();
            foreach (Songbook songbook in MainCatalog.Songbooks)
            {
                TreeNode bookNode = new TreeNode();
                bookNode.Text = songbook.Title;
                bookNode.Tag = new TreeTag(songbook.Filename, -1, TagType.Book); // Sentinel for "open book"
                foreach (Song song in songbook.Songs)
                {
                    TreeNode songNode = new TreeNode();
                    songNode.Tag = new TreeTag(songbook.Filename, song.Page, TagType.Song);
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
    }
}