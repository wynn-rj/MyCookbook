using MyCookbook.Properties;
using RJWynn.Debugging;
using RJWynn.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyCookbook
{
    //TODO: Change resizing


    partial class ShoppingListForm : Form
    {
        private int currentUseHeight = 0;

        List<String> ListedIngredients = new List<String>();
        List<IngredientLine> IngredientLineList = new List<IngredientLine>();
        Recipe[] Recipes;

        public Panel IngredientPanel = new Panel();

        public ShoppingListForm(Recipe[] RecipeList)
        {
            Recipes = RecipeList;

            InitializeComponent();

            this.DialogResult = DialogResult.No;
            this.Name = "panelShoppingList";
            this.DoubleBuffered = true;

            recipeSearchBar.searchBarTextBox.ApplyWatermark("Add Recipe");

            foreach (Recipe rec in RecipeList)
            {
                recipeSearchBar.AddItem(rec.recipeName);
            }

            recipeSearchBar.searchBarTextBox.KeyPress += SearchBarTextBox_KeyPress;
            recipeSearchBar.OnSelection += SearchBar_OnSelection;

            numericUpDown1.Value = 1;

            IngredientPanel.Location = new Point(this.ClientRectangle.Left, toolStrip1.Bottom);
            IngredientPanel.Size = new Size(this.ClientSize.Width, this.ClientRectangle.Bottom - toolStrip1.Bottom);
            this.Controls.Add(IngredientPanel);
        }

        public void PrepPrint()
        {
            this.BackColor = Color.White;
            foreach(IngredientLine ingLine in IngredientLineList)
            {
                ingLine.BackColor = Color.White;
                ingLine.removeButton.Image = Resources.checkbox;
            }
        }

        private void SearchBarTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            EventControls.ValidateKeyPress(e, recipeSearchBar.searchBarTextBox.Text, recipeSearchBar.searchBarTextBox.SelectionStart);
            if (recipeSearchBar.searchBarTextBox.Text.Length <= 1)
            {
                recipeSearchBar.Height = 21;
            }
        }

        private void SearchBar_OnSelection(object sender, EventArgs e)
        {
            AddRecipe(recipeSearchBar.searchBarListBox.SelectedItem.ToString());
            recipeSearchBar.Height = 21;
        }

        private void AddRecipe(string selectedItem)
        {
            Recipe selectedRecipe = null;
            foreach(Recipe rec in Recipes)
            {
                if(rec.recipeName == selectedItem)
                {
                    selectedRecipe = rec;
                }
            }

            if (selectedRecipe != null)
            {
                foreach (Ingredient ing in selectedRecipe.ingredientList)
                {
                    string amount = ing.amount;
                    if (amount == "Amt.")
                        amount = "0";

                    float forDebug = Units.GetValueOfUnit(ing.unitType);

                    if (!ListedIngredients.Contains(ing.name) && Units.GetValueOfUnit(ing.unitType) > 0)
                    {
                        currentUseHeight += 10;
                        ListedIngredients.Add(ing.name);
                        MyDebug.Print(String.Format("Combining ingredient {0} for Unit {1}", ing.name, ing.unitType));
                        IngredientLine temp = new IngredientLine(ing.name, CombineValues("0", amount, (float)numericUpDown1.Value), ing.unitType, currentUseHeight);
                        IngredientLineList.Add(temp);
                        this.IngredientPanel.Controls.Add(temp);
                        currentUseHeight += 32;
                    }
                    else
                    {
                        if(Units.GetValueOfUnit(ing.unitType) > 0)
                        {
                            float unitVal = Units.GetValueOfUnit(ing.unitType);
                            int index = -1;
                            float searchedUnitVal;

                            for (int i = 0; i < ListedIngredients.Count; i++)
                            {
                                if (ListedIngredients[i] == ing.name)
                                    index = i;
                            }

                            if(index > -1)
                            {
                                searchedUnitVal = Units.GetValueOfUnit(IngredientLineList[index].UnitType);
                                if(searchedUnitVal == unitVal)
                                {
                                    IngredientLineList[index].Value = CombineValues(IngredientLineList[index].Value, CombineValues("0", amount, (float)numericUpDown1.Value));
                                }
                                else
                                {
                                    IngredientLineList[index].Value = CombineValues(IngredientLineList[index].Value, CombineValues("0", amount, (float)numericUpDown1.Value), unitVal / searchedUnitVal);
                                }
                            }
                            else
                            {
                                MyDebug.Error("Ingredient listed not found", "ShoppingListForm.AddRecipe", 135);
                            }
                        }
                        else
                        {
                            currentUseHeight += 10;
                            ListedIngredients.Add(ing.name + " (Whole)");
                            MyDebug.Print(String.Format("Combining ingredient {0} for Unit {1}", ing.name + " (Whole)", ing.unitType));
                            IngredientLine temp = new IngredientLine(ing.name + " (Whole)", CombineValues("0", amount, (float)numericUpDown1.Value), ing.unitType, currentUseHeight);
                            IngredientLineList.Add(temp);
                            this.IngredientPanel.Controls.Add(temp);
                            currentUseHeight += 32;
                        }
                    }
                }
            } 
            else
            {
                MyDebug.Error("The selected recipe had a null value", "ShoppingListForm.AddRecipe", 153);
            }
        }

        private string CombineValues(string value, string amount, float v)
        {
            MyDebug.Print(String.Format("Amount: {0} | Value: {1} | Modifier {2}", amount, value, v));

            try
            { 
                string newAmount;
                if(amount.Contains('/'))
                {
                    string[] splitVals = amount.Split('/');
                    newAmount = (int.Parse(splitVals[0]) / int.Parse(splitVals[1]) * v).ToString();
                }
                else
                {
                    newAmount = (Single.Parse(amount) * v).ToString();
                }

                

                return CombineValues(value, newAmount);
            }
            catch
            {
                MyDebug.Error("Parsing Error", "ShoppingListForm.CombineValues", 180);
                string message = "Values couldn't be combined";
                string caption = "Combine error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, buttons);
                this.Close();
                return null;
            }
        }

        private string CombineValues(string value, string amount)
        {
            MyDebug.Print(String.Format("Amount: {0} | Value: {1}", amount, value));
            try
            {
                if (amount.Contains('/'))
                {
                    string[] splitAmts = amount.Split('/');
                    amount = (int.Parse(splitAmts[0]) / int.Parse(splitAmts[1])).ToString();
                }

                if (value.Contains('/'))
                {
                    string[] splitVals = value.Split('/');
                    value = (int.Parse(splitVals[0]) / int.Parse(splitVals[1])).ToString();
                }
                MyDebug.Print(String.Format("Return: {0}", Single.Parse(amount) + Single.Parse(value)));
                return (Single.Parse(amount) + Single.Parse(value)).ToString();
            }
            catch
            {
                MyDebug.Error("Parsing Error", "ShoppingListForm.CombineValues", 212);
                string message = "Values couldn't be combined";
                string caption = "Combine error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, buttons);
                this.Close();
                return null;
            }
        }

        public void RemoveIngredient(IngredientLine ingLine)
        {
            ingLine.Visible = ingLine.Enabled = false;

            int index = IngredientLineList.IndexOf(ingLine);

            ingLine.Dispose();

            IngredientLineList.RemoveAt(index);
            ListedIngredients.RemoveAt(index);

            for (int i = index; i < IngredientLineList.Count; i++)
            {
                IngredientLineList[i].Location = new Point(IngredientLineList[i].Location.X, IngredientLineList[i].Location.Y - 42);
            }
        }
        
        private void ShoppingListForm_Resize(object sender, EventArgs e)
        {
            this.Width = 560;
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }

    public class IngredientLine : Panel
    {
        Label nameLabel = new Label();
        Label valueLabel = new Label();
        Label unitTypeLabel = new Label();
        public PictureBox removeButton = new PictureBox();

        public string IngName { get; private set; }
        public string Value {
            get
            {
                return valueLabel.Text;
            }
            set
            {
                valueLabel.Text = value;
            }
        }
        public string UnitType { get; private set; }

        public IngredientLine(string name, string value, string unitType, int height)
        {
            this.IngName = name;
            this.Value = value;
            this.UnitType = unitType;

            nameLabel.Text = name;
            valueLabel.Text = value;
            unitTypeLabel.Text = unitType;
            
            this.Location = new Point(0, height);
            this.Size = new Size(560, 32);
            this.Controls.AddRange(new Control[4] { nameLabel, valueLabel, unitTypeLabel, removeButton });

            nameLabel.Location = new Point(10, 0);
            nameLabel.Size = new Size(220, 32);
            nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            valueLabel.Location = new Point(nameLabel.Right + 10, 0);
            valueLabel.Size = new Size(80, 32);
            valueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            unitTypeLabel.Location = new Point(valueLabel.Right + 10, 0);
            unitTypeLabel.Size = new Size(160, 32);
            unitTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            removeButton.Location = new Point(unitTypeLabel.Right + 10, 0);
            removeButton.Size = new Size(32, 32);
            removeButton.Image = Resources.Grey_X;
            removeButton.MouseEnter += RemoveButton_MouseEnter;
            removeButton.MouseLeave += RemoveButton_MouseLeave;
            removeButton.Click += RemoveButton_Click;

            this.BackColor = SystemColors.Control;

        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            ((ShoppingListForm)this.Parent.Parent).RemoveIngredient(this);
        }

        private void RemoveButton_MouseLeave(object sender, EventArgs e)
        {
            removeButton.Image = Resources.Grey_X;
        }

        private void RemoveButton_MouseEnter(object sender, EventArgs e)
        {
            removeButton.Image = Resources.Red_X;
        }
    }
}
