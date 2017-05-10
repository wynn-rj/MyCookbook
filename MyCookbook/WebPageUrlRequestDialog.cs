using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCookbook
{
    public partial class WebPageUrlRequestDialog : Form
    {
        public WebPageUrlRequestDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(urlTextBox.Text.Equals(""))
            {
                string message = "Please enter a URL into the textbox";
                string caption = "Enter URL";
                MessageBoxButtons buttons = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, buttons);
            } 
            else
            {
                this.Close();
            }
        }
    }
}
