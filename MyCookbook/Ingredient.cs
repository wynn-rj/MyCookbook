using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RJWynn.Forms;
using System.IO;
using RJWynn.Debugging;

namespace MyCookbook
{
    class Ingredient
    {

        #region Class Variable Declerations
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static int indexCounter;
        public const int height = 21;
        public static int width = 350;
        public static List<string> ingredientDatabase = new List<string>();

        private static int lastSelectedIndex = -1;

        public int x { get; private set; }
        public int y { get; private set; }
        public int index { get; private set; }
        public string name { get; private set; }
        public string amount { get; private set; }
        public string unitType { get; private set; }
        public float unitValue { get; private set; }
        public ComboBox unitComboBox { get; private set; }
        public WatermarkTextBox quantityWatermarkTextbox { get; private set; }
        public SearchBar ingredientNameSearchBox { get; private set; }

        public Panel ingredientPanel { get; private set; }
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion Class Variable Declerations

        /// <summary>
        /// Creates an Ingredient with default values
        /// </summary>
        /// <see cref="Ingredient"/>
        /// <param name="x"><c>ingredientPanel</c> X Coordinate</param>
        /// <param name="y"><c>ingredientPanel</c> Y Coordinate</param>
        public Ingredient(int x, int y) : this(x, y, "", "Amt.", "") { }


        /// <summary>
        /// Creates an Ingredient with predefined values
        /// </summary>
        /// <see cref="MainForm.OpenCookbook()"/>
        /// <param name="x"><c>ingredientPanel</c> X Coordinate</param>
        /// <param name="y"><c>ingredientPanel</c> Y Coordinate</param>
        /// <param name="name">Name of the ingredient</param>
        /// <param name="amount">The quantity of the ingredient</param>
        /// <param name="unitType">The unit used for the quantity</param>
        public Ingredient(int x, int y, string name, string amount, string unitType)
        {
            this.x = x;
            this.y = y;
            this.index = indexCounter++;
            this.name = name;
            if (amount == "0")
            {
                amount = "Amt.";
            }
            this.amount = amount;
            this.unitType = unitType;
            this.unitValue = Units.GetValueOfUnit(unitType);                    
            

            //
            //  Generate form controls
            //
            ingredientPanel = new Panel();
            ingredientNameSearchBox = new SearchBar();
            //ingredientNameSearchBox.UseSearchButton = false;

            quantityWatermarkTextbox = new WatermarkTextBox();
            unitComboBox = new ComboBox();
            ingredientPanel.SuspendLayout();
            // 
            // panel1
            // 
            ingredientPanel.Controls.Add(unitComboBox);
            ingredientPanel.Controls.Add(quantityWatermarkTextbox);
            ingredientPanel.Controls.Add(ingredientNameSearchBox);
            ingredientPanel.Location = new System.Drawing.Point(x, y);
            ingredientPanel.Name = "ingredientPanel" + index;
            ingredientPanel.Size = new System.Drawing.Size(width, 21);
            ingredientPanel.TabIndex = 3;
            ingredientPanel.BackColor = System.Drawing.Color.Transparent;
            ingredientPanel.ForeColor = System.Drawing.Color.Transparent;
            
            // 
            // ingredientNameSearchBox
            // 
            ingredientNameSearchBox.UseSearchButton = false;
            ingredientNameSearchBox.searchBarButton.Dispose();
            ingredientNameSearchBox.Location = new System.Drawing.Point(0, 0);
            ingredientNameSearchBox.Name = "ingredientNameSearchBox" + index;
            ingredientNameSearchBox.Size = new System.Drawing.Size(width - 160, 21);
            ingredientNameSearchBox.searchBarTextBox.Text = name;
            ingredientNameSearchBox.searchBarTextBox.Width = width - 160;
            ingredientNameSearchBox.TabIndex = 0;
            ingredientNameSearchBox.searchBarTextBox.KeyPress += IngredientNameTextBox_KeyPress;
            ingredientNameSearchBox.searchBarTextBox.GotFocus += SearchBarTextBox_GotFocus;
            ingredientNameSearchBox.Resize += SearchBar_Resize;
            ingredientNameSearchBox.searchBarListBox.Click += SearchBarListBox_Click;
            ingredientNameSearchBox.searchBarTextBox.WatermarkText = "Ingredient Name";
            if (name != "Ingredient Name")
            {
                ingredientNameSearchBox.searchBarTextBox.WatermarkActive = false;
                ingredientNameSearchBox.searchBarTextBox.ForeColor = System.Drawing.Color.Black;

            }
            
            ingredientNameSearchBox.AddItems(ingredientDatabase.ToArray());
            // 
            // quantityWatermarkTextbox
            // 
            quantityWatermarkTextbox.Location = new System.Drawing.Point(ingredientNameSearchBox.Right + 10, 0);
            quantityWatermarkTextbox.Name = "quantityWatermarkTextbox" + index;
            quantityWatermarkTextbox.Size = new System.Drawing.Size(40, 21);
            quantityWatermarkTextbox.TextAlign = HorizontalAlignment.Center;            
            quantityWatermarkTextbox.Text = amount;
            quantityWatermarkTextbox.WatermarkText = "Amt.";
            if (amount != "Amt.")
            {
                quantityWatermarkTextbox.WatermarkActive = false;
                quantityWatermarkTextbox.ForeColor = System.Drawing.Color.Black;

            }
            quantityWatermarkTextbox.TabIndex = 1;
            quantityWatermarkTextbox.TextChanged += QuantityWatermarkTextbox_TextChanged;
            quantityWatermarkTextbox.KeyPress += QuantityWatermarkTextbox_KeyPress;
            // 
            // unitComboBox
            // 
            unitComboBox.FormattingEnabled = true;
            unitComboBox.Items.AddRange(Units.unitNames);
            unitComboBox.Location = new System.Drawing.Point(quantityWatermarkTextbox.Right + 10, 0);
            unitComboBox.Name = "unitComboBox" + index;
            unitComboBox.Size = new System.Drawing.Size(100, 21);
            unitComboBox.Text = unitType;
            unitComboBox.TabIndex = 2;
            unitComboBox.KeyPress += UnitComboBox_KeyPress;

            ingredientPanel.ResumeLayout(false);
            ingredientPanel.PerformLayout();

            
        }

        

        private void QuantityWatermarkTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && 
                !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.') && 
                (e.KeyChar != '/'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            // only allow one decimal /
            if ((e.KeyChar == '/') && ((sender as TextBox).Text.IndexOf('/') > -1))
            {
                e.Handled = true;
            }

            // makes sure their is a number before the /
            if ((e.KeyChar == '/') && ((sender as TextBox).Text.Length < 1))
            {
                e.Handled = true;
            }
        }

        private void SearchBarListBox_Click(object sender, EventArgs e)
        {
            MyDebug.Print("Ingredient Listbox Clicked");
            ingredientNameSearchBox.searchBarTextBox.Text = ingredientNameSearchBox.Selected.ToString();
            ingredientNameSearchBox.ClearItems(false);
            ingredientPanel.Height = 21;
        }

        private void SearchBar_Resize(object sender, EventArgs e)
        {
            
            if (ingredientNameSearchBox.searchBarListBox.Visible)
            {
                //ingredientNameSearchBox.searchBarListBox.BringToFront();
                //ingredientNameSearchBox.BringToFront();

                ingredientNameSearchBox.Width = ingredientPanel.Width;
                ingredientNameSearchBox.searchBarTextBox.Width = ingredientPanel.Width - 160;
                ingredientNameSearchBox.searchBarListBox.Width = ingredientPanel.Width;
                ingredientPanel.Height = ingredientNameSearchBox.searchBarListBox.Height + 21;
                
            }
            else
            {
                ingredientNameSearchBox.searchBarTextBox.Width = ingredientNameSearchBox.Width = ingredientPanel.Width - 160;
            }
            
        }



        #region Ingredient Events
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void UnitComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            EventControls.IgnoreKeyPress(e, '"');            
        }

        private void IngredientNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            EventControls.ValidateKeyPress(e, ingredientNameSearchBox.searchBarTextBox.Text, ingredientNameSearchBox.searchBarTextBox.SelectionStart);
            if (ingredientNameSearchBox.searchBarTextBox.Text.Length <= 1)
            {
                ingredientPanel.Height = 21;
            }
        }        

        private void QuantityWatermarkTextbox_TextChanged(object sender, EventArgs e)
        {
            this.amount = quantityWatermarkTextbox.Text;
        }

        public void IngredientLostFocus(object sender, EventArgs e)
        {
            this.name = ingredientNameSearchBox.searchBarTextBox.Text;
            ingredientPanel.Height = 21;
        }

        private void SearchBarTextBox_GotFocus(object sender, EventArgs e)
        {
            if (lastSelectedIndex != -1)
            {
                try
                {
                    Panel p = (Panel) ingredientPanel.Parent.Controls.Find("ingredientPanel" + lastSelectedIndex, false)[0];
                    p.Height = 21;
                    SearchBar s = (SearchBar)(p.Controls.Find("ingredientNameSearchBox" + lastSelectedIndex, true)[0]);
                    s.ClearItems(false);
                }
                catch (Exception)
                {

                }
            }
            lastSelectedIndex = index;
            this.UpdateRefrence();            
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion Ingredient Events

        /// <summary>
        /// Updates the class variables from the form controls
        ///     <paramref name="name"/>
        ///     <paramref name="amount"/>
        ///     <paramref name="unitType"/>
        /// </summary>
        public void UpdateVariables()
        {
            if (this.name != ingredientNameSearchBox.searchBarTextBox.Text ||
                this.amount != quantityWatermarkTextbox.Text ||
                this.unitType != unitComboBox.Text
                )
            { 
                this.name = ingredientNameSearchBox.searchBarTextBox.Text;
                this.amount = quantityWatermarkTextbox.Text;
                this.unitType = unitComboBox.Text;

                ((MainForm)ingredientPanel.Parent.Parent.Parent).needSave = true;
                MyDebug.Print("Update needed: " + this.ingredientPanel.Name);
            }
        }

        /// <summary>
        /// Resizes the ingredient and moves it when thre Resize event is called
        /// <see cref="MainForm.Form1_Resize(object, EventArgs)"/>
        /// </summary>
        /// <param name="x">Changed X</param>
        /// <param name="y">Changed Y</param>
        public void IngredientResize(int x, int y)
        {
            ingredientPanel.Location = new System.Drawing.Point(x, y);
            ingredientPanel.Size = new System.Drawing.Size(width, 21);

            ingredientNameSearchBox.Location = new System.Drawing.Point(0, 0);
            ingredientNameSearchBox.Size = new System.Drawing.Size(width - 160, 21);

            quantityWatermarkTextbox.Location = new System.Drawing.Point(ingredientNameSearchBox.Right + 10, 0);
            quantityWatermarkTextbox.Size = new System.Drawing.Size(40, 21);

            unitComboBox.Location = new System.Drawing.Point(quantityWatermarkTextbox.Right + 10, 0);
            unitComboBox.Size = new System.Drawing.Size(100, 21);
        }

        public void UpdateRefrence()
        {
            ingredientNameSearchBox.ClearItems(false);
            ingredientNameSearchBox.AddItems(ingredientDatabase.ToArray());
            
        }

        /// <summary>
        /// Updates Field values and formats them to a string
        /// </summary>
        /// <returns>Ingredient Fields seperated by semicolons</returns>
        public override string ToString()
        {
            UpdateVariables();
            string delimiter = @""",""";
            return @"""" + name + delimiter + amount + delimiter + unitType + @"""";
        }

        public static string[] OpenIngredientDatabase(string filePath)
        {
            List<byte> fileDump = new List<byte>();
            if(!File.Exists(filePath + @"\IngredientDatabase.csv"))
            {
                FileStream f = File.Create(filePath + @"\IngredientDatabase.csv");
                f.Close();
            }
            using (Stream sr = File.OpenRead(filePath + @"\IngredientDatabase.csv"))
            {
                int temp;
                temp = sr.ReadByte();   //ReadByte returns an int for some reason. Is casted later
                do
                {
                    fileDump.Add((byte)temp);   //Casts int into byte that was read
                    temp = sr.ReadByte();       //Reads next byte
                } while (temp != -1);   //Runs until file is empty
            }
            if (fileDump.Count > 1)
            {
                //Converts byte array to string
                //GetString is in recipe because that is where it was first implemented, not because it uses anything from recipe.
                string fileDumpString = Recipe.GetString(fileDump.ToArray());

                List<char> stringEdit = fileDumpString.ToCharArray().ToList();

                stringEdit.RemoveAt(0);                   //Removes First Quote
                stringEdit.RemoveAt(stringEdit.Count - 1);  //Removes last quote

                fileDumpString = new string(stringEdit.ToArray());

                //Splits the string into an array based of the seperator semicolon
                string[] args = fileDumpString.Split(new string[] { @""",""" }, StringSplitOptions.None);                

                return args;
            }
            return null;
        }

        public static void SaveIngredientDatabase(string filePath)
        {
            MyDebug.Print("Save Database Called");
            MyDebug.Print(filePath + @"\IngredientDatabase.csv");
            string[] alreadyInFile = OpenIngredientDatabase(filePath);
            using (Stream sr = File.OpenWrite(filePath + @"\IngredientDatabase.csv"))
            {
                
                if (ingredientDatabase.Count > 0)
                {
                    foreach (string str in ingredientDatabase)
                    {
                        if (alreadyInFile != null)
                        {
                            if (!alreadyInFile.Contains(str))
                            {
                                MyDebug.Print("Writing: " + @",""" + str + @"""");
                                byte[] toWrite = Recipe.GetBytes(@",""" + str + @"""");
                                sr.Write(toWrite, 0, toWrite.Length);
                            }
                        }
                        else
                        {
                            MyDebug.Print("Writing: " + @"""" + str + @"""");
                            byte[] toWrite = Recipe.GetBytes(@"""" + str + @"""");
                            sr.Write(toWrite, 0, toWrite.Length);
                            if(str != ingredientDatabase.Last())
                            {
                                byte[] toWriteComma = Recipe.GetBytes(",");
                                sr.Write(toWriteComma, 0, toWriteComma.Length);
                            }
                        }
                        
                    }
                }
                
            }
        }


    }
}
