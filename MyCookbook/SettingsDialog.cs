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
    public partial class SettingsDialog : Form
    {
        List<CheckBox> settings = new List<CheckBox>();

        public bool[] ReturnSettings 
        {
            get
            {
                List<bool> toArray = new List<bool>();
                foreach (CheckBox checkBox in settings)
                {
                    toArray.Add(checkBox.Checked);
                }

                return toArray.ToArray();
            }
        }

        public SettingsDialog(bool[] values)
        {
            

            bool searchIngredients = values[0];

            InitializeComponent();
            AddSetting("Include ingredients in search", searchIngredients);
        }

        private void AddSetting(string text, bool startValue)
        {

            this.Height += 20;

            CheckBox checkBox1 = new CheckBox();

            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(13, this.ClientSize.Height - 20);
            checkBox1.Name = "checkBox" + text;
            checkBox1.Size = new System.Drawing.Size(200, 17);
            checkBox1.TabIndex = 0;
            checkBox1.Text = text;
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.Checked = startValue;

            this.Controls.Add(checkBox1);
            settings.Add(checkBox1);
        }
    }
}
