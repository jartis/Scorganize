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
        private string tempfile;

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
                        Setlist l = JsonSerializer.Deserialize<Setlist>(jsonString);
                        foreach (SetlistEntry entry in l.Entries)
                        {
                            SetlistListbox.Items.Add(entry);
                            setlist.Add(entry);
                        }
                    }
                }
            }
            PdfDocument? setlistDoc = new PdfDocument();
            foreach (SetlistEntry entry in setlist.Entries)
            {
                using (PdfDocument bookDoc = PdfReader.Open(entry.Filename, PdfDocumentOpenMode.Import))
                {
                    for (int pg = entry.StartPage; pg < entry.StartPage + entry.NumPages; pg++)
                    {
                        setlistDoc.AddPage(bookDoc.Pages[pg-1]);
                    }
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
            curDoc.Dispose();
            File.Delete(tempfile);
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
            curPage = Math.Min(curPage + 1, curDoc.PageCount - 1);
            Invalidate();
        }

        private void PageBack()
        {
            curPage = Math.Max(curPage - 1, 1);
            Invalidate();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (curDoc != null)
            {
                LeftBox.Image = curDoc.Render(Math.Max(curPage - 1, 0), LeftBox.Width, LeftBox.Height, leftG.DpiX, leftG.DpiY, false);
                RightBox.Image = curDoc.Render(Math.Min(curPage, curDoc.PageCount), RightBox.Width, RightBox.Height, rightG.DpiX, rightG.DpiY, false);
            }
            base.OnPaint(e);
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
    }
}