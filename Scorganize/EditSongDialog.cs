using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scorganize
{
    public partial class EditSongDialog : Form
    {
        public string SongTitle { get; set; }
        public string SongArtist { get; set; }

        public int SongPage { get; set; }

        public int NumPages { get; set; }

        public EditSongDialog()
        {
            InitializeComponent();
            SongTitle = String.Empty;
            SongArtist = String.Empty;
            this.SongArtistTextBox.KeyDown += TextBox_KeyDown;
            this.SongTitleTextBox.KeyDown += TextBox_KeyDown;
            this.SongTitleTextBox.TextChanged += (sender, args) => { SongTitle = this.SongTitleTextBox.Text; };
            this.SongArtistTextBox.TextChanged += (sender, args) => { SongArtist = this.SongArtistTextBox.Text; };
            this.PageNumberInput.ValueChanged += (sender, args) => { SongPage = (int)this.PageNumberInput.Value; };
            this.NumPagesControl.ValueChanged += (sender, args) => { NumPages = (int)this.NumPagesControl.Value; };
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                SaveButton.PerformClick();
            }
        }

        public EditSongDialog(string title, string artist, int page, int numpages)
        {
            InitializeComponent();
            this.SongTitleTextBox.Text = title;
            SongTitle = title;
            this.SongArtistTextBox.Text = artist;
            SongArtist = artist;
            this.PageNumberInput.Value = page;
            SongPage = page;
            this.NumPagesControl.Value = numpages;
            NumPages = numpages;
            this.SongTitleTextBox.TextChanged += (sender, args) => { SongTitle = this.SongTitleTextBox.Text; };
            this.SongArtistTextBox.TextChanged += (sender, args) => { SongArtist = this.SongArtistTextBox.Text; };
            this.PageNumberInput.ValueChanged += (sender, args) => { SongPage = (int)this.PageNumberInput.Value; };
            this.PageNumberInput.ValueChanged += (sender, args) => { NumPages = (int)this.NumPagesControl.Value; };
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
