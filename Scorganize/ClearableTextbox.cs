using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorganize
{
    public class ClearableTextBox : TextBox
    {
        private readonly Label ClearLabel;

        public bool ButtonTextClear { get; set; } = true;

        public ClearableTextBox()
        {
            Resize += PositionX;

            TextChanged += ShowHideX;

            ClearLabel = new Label()
            {
                Location = new Point(100, 0),
                AutoSize = true,
                Text = "✕",
                ForeColor = Color.Gray,
                Visible = false,
                Cursor = Cursors.Arrow
            };

            Controls.Add(ClearLabel);
            ClearLabel.Click += (sender, e) => { Text = string.Empty; };
            ClearLabel.BringToFront();
        }



        private void ShowHideX(object? sender, EventArgs e) => ClearLabel.Visible = ButtonTextClear && !string.IsNullOrEmpty(Text);
        private void PositionX(object? sender, EventArgs e) => ClearLabel.Location = new Point(Width - 15, ((Height - ClearLabel.Height) / 2) - 3);
    }
}
