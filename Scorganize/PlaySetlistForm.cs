using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System.Configuration;
using System.Text.Json;

namespace Scorganize
{
    public partial class PlaySetlistForm : Form
    {
        public Setlist setlist { get; set; }

        private PdfiumViewer.PdfRenderer pdfRenderer;
        private Graphics leftG;
        private Graphics rightG;
        private PdfiumViewer.PdfDocument? curDoc;
        private int curPage = 1;
        private int pagedelta = 1;
        private string tempfile;
        private PageDisplay pd;

        public PlaySetlistForm()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(PlaySetlistForm_KeyDown);
            Cursor.Current = Cursors.Default;
            pdfRenderer = new PdfiumViewer.PdfRenderer();
            leftG = LeftBox.CreateGraphics();
            rightG = RightBox.CreateGraphics();
            this.splitContainer1.SplitterMoved += new SplitterEventHandler((sender, e) => this.Invalidate());
            this.PreviewKeyDown += new PreviewKeyDownEventHandler(PlaySetlistForm_PreviewKeyDown);
            this.MouseWheel += PlaySetlistForm_MouseWheel;
            this.FormClosing += PlaySetlistForm_FormClosing;
            this.SetlistListbox.DisplayMember = "Title";
            setlist = new Setlist();
            tempfile = String.Empty;
            pd = PageDisplay.Double;
            sideBySideViewToolStripMenuItem.Click += PageViewClickHandler;
            singlePageViewToolStripMenuItem.Click += PageViewClickHandler;
            SinglePageScrollMenuItem.Click += ScrollClickHandler;
            TwoPageScrollMenuItem.Click += ScrollClickHandler;
            SetlistListbox.Click += SetlistListbox_Click;
        }

        private void SetlistListbox_Click(object? sender, EventArgs e)
        {
            if (SetlistListbox.SelectedItems.Count == 0)
            {
                return;
            }
            curPage = ((SetlistEntry)(SetlistListbox.SelectedItem)).SetlistPage;
            Invalidate();
        }

        private void ScrollClickHandler(object? sender, EventArgs e)
        {
            if (sender == SinglePageScrollMenuItem)
            {
                pagedelta = 1;
                SinglePageScrollMenuItem.Checked = true;
                TwoPageScrollMenuItem.Checked = false;
            }
            else if (sender == TwoPageScrollMenuItem)
            {
                pagedelta = 2;
                SinglePageScrollMenuItem.Checked = false;
                TwoPageScrollMenuItem.Checked = true;
            }
        }

        // TODO: Add internal bookmarks to this document, in case of exporting
        public void LoadSetlistAndBuildDoc()
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
                            SetlistListbox.Items.Add(entry);
                            setlist.Add(entry);
                        }
                    }
                }
            }
            PdfDocument? setlistDoc = new PdfDocument();
            int setlistBookPage = 0;
            foreach (SetlistEntry entry in setlist.Entries)
            {
                using (PdfDocument bookDoc = PdfReader.Open(entry.Filename, PdfDocumentOpenMode.Import))
                {
                    for (int pg = entry.StartPage; pg < entry.StartPage + entry.NumPages; pg++)
                    {
                        setlistDoc.AddPage(bookDoc.Pages[pg-1]);
                        setlistBookPage++;
                    }
                    setlistDoc.Outlines.Add(new PdfOutline(entry.Title, setlistDoc.Pages[setlistBookPage - 1], true));
                }
            }
            tempfile = Path.GetTempFileName();
            setlistDoc.Save(tempfile);
            setlistDoc.Dispose();
            curDoc = PdfiumViewer.PdfDocument.Load(tempfile);
            curPage = 1;
            Invalidate();
        }

        private void PlaySetlistForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (curDoc != null)
            {
                curDoc.Dispose();
            }
            if (File.Exists(tempfile))
            {
                File.Delete(tempfile);
            }
        }

        private void PlaySetlistForm_MouseWheel(object? sender, MouseEventArgs e)
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

        private void PlaySetlistForm_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
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

        private void PlaySetlistForm_KeyDown(object? sender, KeyEventArgs e)
        {
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
            if (curDoc is null) { return; }
            curPage = Math.Min(curPage + pagedelta, curDoc.PageCount - 1);
            Invalidate();
        }

        private void PageBack()
        {
            if (curDoc is null) { return; }
            curPage = Math.Max(curPage - pagedelta, 1);
            Invalidate();
        }

        #region Event Handlers

        protected override void OnPaint(PaintEventArgs e)
        {
            if (curDoc != null)
            {
                if (pd == PageDisplay.Single)
                {
                    LeftBox.Image = curDoc.Render(Math.Min(Math.Max(curPage, 0), curDoc.PageCount-1), 150f, 150f, false);
                }
                else if (pd == PageDisplay.Double)
                {
                    //var width = LeftBox.Width;
                    //var height = LeftBox.Height;
                    LeftBox.Image = curDoc.Render(Math.Min(Math.Max(curPage, 0), curDoc.PageCount-1), 150f, 150f, false);
                    RightBox.Image = curDoc.Render(Math.Min(Math.Max(curPage+1, 0), curDoc.PageCount-1), 150f, 150f, false);
                }
            }
            base.OnPaint(e);
        }

        private void PageViewClickHandler(object? sender, EventArgs e)
        {
            if (sender == singlePageViewToolStripMenuItem)
            {
                pd = PageDisplay.Single;
                singlePageViewToolStripMenuItem.Checked = true;
                sideBySideViewToolStripMenuItem.Checked = false;
                RightBox.Visible = false;

                // Don't scroll two pages when in single view
                pagedelta = 1;
                SinglePageScrollMenuItem.Checked = true;
                TwoPageScrollMenuItem.Checked = false;
                TwoPageScrollMenuItem.Enabled = false;
            }
            else if (sender == sideBySideViewToolStripMenuItem)
            {
                pd = PageDisplay.Double;
                singlePageViewToolStripMenuItem.Checked = false;
                sideBySideViewToolStripMenuItem.Checked = true;
                RightBox.Visible = true;
                TwoPageScrollMenuItem.Enabled = true;
            }
            UpdateTabPanel();
        }

        private void UpdateTabPanel()
        {
            if (pd == PageDisplay.Single)
            {
                this.DisplayTable.ColumnCount = 1;
            }
            else if (pd == PageDisplay.Double)
            {
                this.DisplayTable.ColumnCount = 2;
            }
            Invalidate();
        }

        private void ForwardBtn_Click(object sender, EventArgs e)
        {
            PageForward();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            PageBack();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExportPdfButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Setbook|*.pdf";
            saveFileDialog.Title = "Save Setbook";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                try
                {
                    File.Copy(tempfile, saveFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Error saving setbook");
                }
            }
        }

        #endregion
    }
}