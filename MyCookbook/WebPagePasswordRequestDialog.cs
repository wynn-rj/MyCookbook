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
    public partial class WebPagePasswordRequestDialog : Form
    {
        public WebPagePasswordRequestDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(usernameTextBox.Text.Equals("") || passwordTextBox.Text.Equals(""))
            {
                string message = "Please enter a Username and Password above";
                string caption = "Enter Username and Password";
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
