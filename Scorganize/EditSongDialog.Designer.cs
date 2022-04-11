namespace Scorganize
{
    partial class EditSongDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SongTitleTextBox = new System.Windows.Forms.TextBox();
            this.SongArtistTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.PageNumberInput = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.PageNumberInput)).BeginInit();
            this.SuspendLayout();
            // 
            // SongTitleTextBox
            // 
            this.SongTitleTextBox.Location = new System.Drawing.Point(53, 12);
            this.SongTitleTextBox.Name = "SongTitleTextBox";
            this.SongTitleTextBox.Size = new System.Drawing.Size(459, 23);
            this.SongTitleTextBox.TabIndex = 0;
            // 
            // SongArtistTextBox
            // 
            this.SongArtistTextBox.Location = new System.Drawing.Point(53, 43);
            this.SongArtistTextBox.Name = "SongArtistTextBox";
            this.SongArtistTextBox.Size = new System.Drawing.Size(459, 23);
            this.SongArtistTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Title";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Artist";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(437, 73);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "💾 Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(356, 73);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "❌ Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Page";
            // 
            // PageNumberInput
            // 
            this.PageNumberInput.Location = new System.Drawing.Point(53, 72);
            this.PageNumberInput.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.PageNumberInput.Name = "PageNumberInput";
            this.PageNumberInput.Size = new System.Drawing.Size(47, 23);
            this.PageNumberInput.TabIndex = 8;
            this.PageNumberInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EditSongDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 106);
            this.Controls.Add(this.PageNumberInput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SongArtistTextBox);
            this.Controls.Add(this.SongTitleTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditSongDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Edit Song Information";
            ((System.ComponentModel.ISupportInitialize)(this.PageNumberInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox SongTitleTextBox;
        private TextBox SongArtistTextBox;
        private Label label1;
        private Label label2;
        private Button SaveButton;
        private Button CancelButton;
        private Label label3;
        private NumericUpDown PageNumberInput;
    }
}