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
    public partial class EditBookDialog : Form
    {
        public string BookName { get; set; }
        public string FileName { get; set; }

        public EditBookDialog()
        {
            InitializeComponent();
            this.BookNameTextBox.TextChanged += (sender, args) => { BookName = this.BookNameTextBox.Text; };
            this.BookNameTextBox.KeyDown += TextBox_KeyDown;
            this.FilenameBox.TextChanged += (sender, args) => { FileName = this.FilenameBox.Text; };
            this.FilenameBox.KeyDown += TextBox_KeyDown;
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

        public EditBookDialog(string title, string path)
        {
            InitializeComponent();
            this.BookNameTextBox.Text = title;
            BookName = title;
            this.FilenameBox.Text = path;
            FileName = path;
            this.BookNameTextBox.TextChanged += (sender, args) => { BookName = this.BookNameTextBox.Text; };
            this.FilenameBox.TextChanged += (sender, args) => { FileName = this.FilenameBox.Text; };
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FilePickerButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog? form = new OpenFileDialog())
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.FilenameBox.Text = form.FileName;
                    FileName = form.FileName;
                }
            }
        }
    }
}
