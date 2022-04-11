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

        public EditSongDialog()
        {
            InitializeComponent();
            this.SongTitleTextBox.TextChanged += (sender, args) => { SongTitle = this.SongTitleTextBox.Text; };
            this.SongArtistTextBox.TextChanged += (sender, args) => { SongArtist = this.SongArtistTextBox.Text; };
            this.PageNumberInput.ValueChanged += (sender, args) => { SongPage = (int)this.PageNumberInput.Value; };
        }

        public EditSongDialog(string title, string artist, int page)
        {
            InitializeComponent();
            this.SongTitleTextBox.Text = title;
            SongTitle = title;
            this.SongArtistTextBox.Text = artist;
            SongArtist = artist;
            this.PageNumberInput.Value = page;
            SongPage = page;
            this.SongTitleTextBox.TextChanged += (sender, args) => { SongTitle = this.SongTitleTextBox.Text; };
            this.SongArtistTextBox.TextChanged += (sender, args) => { SongArtist = this.SongArtistTextBox.Text; };
            this.PageNumberInput.ValueChanged += (sender, args) => { SongPage = (int)this.PageNumberInput.Value; };
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
