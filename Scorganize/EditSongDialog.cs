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
            SongTitle = String.Empty;
            SongArtist = String.Empty;
            this.SongTitleTextBox.TextChanged += (sender, args) => { SongTitle = this.SongTitleTextBox.Text; };
            this.SongTitleTextBox.KeyDown += TextBox_KeyDown;
            this.SongArtistTextBox.TextChanged += (sender, args) => { SongArtist = this.SongArtistTextBox.Text; };
            this.SongArtistTextBox.KeyDown += TextBox_KeyDown;
            this.PageNumberInput.ValueChanged += (sender, args) => { SongPage = (int)this.PageNumberInput.Value; };
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveButton.PerformClick();
                // these last two lines will stop the beep sound
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
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
