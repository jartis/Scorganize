using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scorganize
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.Text = "About";
            this.labelProductName.Text = Application.ProductName;
            this.labelVersion.Text = String.Format("Version {0}", Application.ProductVersion);
            this.labelCopyright.Text = "© 2022 J Artis";
            this.labelCompanyName.Text = Application.CompanyName;
            this.textBoxDescription.Text = "Organize your unwieldly pile of completely legitimate PDF songbooks, search and sort songs by name. Special thanks to all my musician and software development friends for making this possible.";
        }
    }
}
