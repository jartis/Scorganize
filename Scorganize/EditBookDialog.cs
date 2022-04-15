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
            this.BookNameTextBox.KeyDown += TextBox_KeyDown;
            this.FilenameBox.KeyDown += TextBox_KeyDown;
            this.BookNameTextBox.TextChanged += (sender, args) => { BookName = this.BookNameTextBox.Text; };
            this.FilenameBox.TextChanged += (sender, args) => { FileName = this.FilenameBox.Text; };
            BookName = String.Empty;
            FileName = String.Empty;
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
