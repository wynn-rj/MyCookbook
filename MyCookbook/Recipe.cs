using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RJWynn.Forms;
using RJWynn.Debugging;

namespace MyCookbook
{
    class Recipe
    {
        //IMPLEMENT:Add comments
        private static int recipeDrawSpacing = 10;
        private static int scrollBarWidth = 20;
        private static int indexLooper;

        private int usableWidth;

        public int temperature;
        public byte hours;
        public float minutes;
        public string description;
        public string recipeName;
        public string cookingUtensil;
        public float servings;
        public string imageLocation;
        private bool useComboBox = false;
        private bool useNumeric = true;


        public int index { get; private set; }

        public TreeNode node { get; private set; }

        public Panel recipePanel { get; private set; }
        public PictureBox recipePictureBox { get; private set; }

        public TextBox recipeNameTextBox { get; private set; }
        public ComboBox cookingUtensilComboBox { get; private set; }
        public NumericUpDown temperatureNumeric { get; private set; }
        public ComboBox temperatureComboBox { get; private set; }
        public Label temperatureLabel { get; private set; }

        public Label timeLabel { get; private set; }
        public NumericUpDown hourNumeric { get; private set; }
        public Label hourLabel { get; private set; }
        public NumericUpDown minuteNumeric { get; private set; }
        public Label minuteLabel { get; private set; }

        public Label servingsLabel { get; private set; }
        public NumericUpDown servingsNumeric { get; private set; }


        public Panel ingredientPanel { get; private set; }

        public WatermarkTextBox descriptionTextBox { get; private set; }

        public List<Ingredient> ingredientList { get; private set; }

        public Button addIngredientButton { get; private set; }
        public Button removeIngredientButton { get; private set; }

        public Recipe(string name, TreeNode node, int width, int height, int x, int y, int temperature, byte hours, float minutes, string description, string cookingUtensil, float servings, string imageLocation, bool addFirstIngredient)
        {
            this.node = node;
            this.recipeName = name;
            this.temperature = temperature;
            this.hours = hours;
            this.minutes = minutes;
            this.description = description;
            this.cookingUtensil = cookingUtensil;
            this.servings = servings;
            this.imageLocation = imageLocation;

            this.node.Name = name;
            this.node.Text = name;

            this.index = indexLooper;
            indexLooper++;



            usableWidth = width - 3 * recipeDrawSpacing;

            ingredientList = new List<Ingredient>();

            recipePanel = new Panel();
            recipePictureBox = new PictureBox();
            recipeNameTextBox = new TextBox();
            cookingUtensilComboBox = new ComboBox();
            temperatureNumeric = new NumericUpDown();
            temperatureComboBox = new ComboBox();
            temperatureLabel = new Label();
            timeLabel = new Label();
            hourNumeric = new NumericUpDown();
            hourLabel = new Label();
            minuteNumeric = new NumericUpDown();
            minuteLabel = new Label();
            servingsLabel = new Label();
            servingsNumeric = new NumericUpDown();
            ingredientPanel = new Panel();
            descriptionTextBox = new WatermarkTextBox();

            addIngredientButton = new Button();
            removeIngredientButton = new Button();

            ((System.ComponentModel.ISupportInitialize)(recipePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(temperatureNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(hourNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(minuteNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(servingsNumeric)).BeginInit();

            recipePanel.SuspendLayout();

            recipePanel.Controls.Add(recipePictureBox);
            recipePanel.Controls.Add(recipeNameTextBox);
            recipePanel.Controls.Add(cookingUtensilComboBox);
            recipePanel.Controls.Add(temperatureNumeric);
            recipePanel.Controls.Add(temperatureLabel);
            recipePanel.Controls.Add(timeLabel);
            recipePanel.Controls.Add(hourNumeric);
            recipePanel.Controls.Add(hourLabel);
            recipePanel.Controls.Add(minuteNumeric);
            recipePanel.Controls.Add(minuteLabel);
            recipePanel.Controls.Add(servingsNumeric);
            recipePanel.Controls.Add(servingsLabel);
            recipePanel.Controls.Add(ingredientPanel);
            recipePanel.Controls.Add(descriptionTextBox);
            recipePanel.Controls.Add(addIngredientButton);
            recipePanel.Controls.Add(removeIngredientButton);
            recipePanel.Controls.Add(temperatureComboBox);


            #region FormGeneration
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //
            //
            //
            recipePanel.Location = new System.Drawing.Point(x, y);
            recipePanel.Name = "panel" + name;
            recipePanel.Size = new System.Drawing.Size(width, height);
            recipePanel.TabIndex = 2;
            recipePanel.AllowDrop = true;
            recipePanel.AutoScroll = true;
            recipePanel.HorizontalScroll.Enabled = false;
            //
            //
            //
            recipePictureBox.Location = new System.Drawing.Point(recipeDrawSpacing, recipeDrawSpacing);
            recipePictureBox.Name = "pictureBox" + name;
            recipePictureBox.Size = new System.Drawing.Size(usableWidth / 2, 200);
            recipePictureBox.TabIndex = 0;
            recipePictureBox.TabStop = false;
            recipePictureBox.ErrorImage = global::MyCookbook.Properties.Resources.imageNotFound;
            recipePictureBox.AllowDrop = true;
            recipePictureBox.Image = global::MyCookbook.Properties.Resources.defaultImage;
            recipePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            if (imageLocation != "")
            {
                try
                {
                    if(imageLocation != recipePictureBox.ImageLocation)
                    {
                        recipePictureBox.Image = Image.FromFile(imageLocation);
                        recipePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                        recipePictureBox.ImageLocation = imageLocation;
                    }
                    
                }
                catch
                {
                    MyDebug.Error("Image Convert failed for file: " + imageLocation, "Recipe (Constructor)", 159);

                    string message = "Could not find the image for this recipe. Has the image been moved or deleted?";
                    string caption = "Image error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;

                    MessageBox.Show(message, caption, buttons);

                    imageLocation = "";
                }
            }
            else
            {
                imageLocation = recipePictureBox.ImageLocation;
            }
            recipePictureBox.BackgroundImage = global::MyCookbook.Properties.Resources.imageNotFound;

            recipePictureBox.DoubleClick += RecipePictureBox_DoubleClick;
            recipePictureBox.DragEnter += RecipePictureBox_DragEnter;
            recipePictureBox.DragDrop += RecipePictureBox_DragDrop;


            /* if (recipePictureBox.Image != null)
             {
                 if (recipePictureBox.Image.Size.Height > recipePictureBox.Size.Height || recipePictureBox.Image.Size.Width > recipePictureBox.Size.Width)
                 {
                     recipePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                 }
                 else
                 {
                     recipePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                 }
             }
             else
             {
                 recipePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
             } */
            //recipePictureBox.DragEnter += RecipePictureBox_DragEnter;
            //recipePictureBox.DragDrop += RecipePictureBox_DragDrop;
            //
            //
            //
            recipeNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            recipeNameTextBox.Location = new System.Drawing.Point(recipePictureBox.Right + recipeDrawSpacing, recipeDrawSpacing);
            recipeNameTextBox.Name = "nameTextBox" + name;
            recipeNameTextBox.Size = new System.Drawing.Size(usableWidth / 2, 44);
            recipeNameTextBox.TabIndex = 1;
            recipeNameTextBox.Text = name;
            recipeNameTextBox.TextChanged += RecipeNameTextBox_TextChanged;
            recipeNameTextBox.KeyPress += RecipeNameTextBox_KeyPress;

            //Used to divide the right half of the panel into 3 parts           110 is the temperature label width
            int halfWidthWithSpacing = (usableWidth / 2) - 2 * recipeDrawSpacing - 110;
            //
            //
            //
            cookingUtensilComboBox.FormattingEnabled = true;
            cookingUtensilComboBox.Text = cookingUtensil;
            cookingUtensilComboBox.Location = new System.Drawing.Point(recipeNameTextBox.Left, recipeNameTextBox.Bottom + recipeDrawSpacing);
            cookingUtensilComboBox.Name = "comboBox" + name;
            cookingUtensilComboBox.Size = new System.Drawing.Size((int)(halfWidthWithSpacing * .5), 21);
            cookingUtensilComboBox.TabIndex = 2;
            cookingUtensilComboBox.Items.AddRange(new String[] { "Conventional Oven", "Convection Oven", "Pan", "Pot", "Toaster", "Microwave", "Griddle", "Bowl" });
            cookingUtensilComboBox.KeyPress += CookingUtensilComboBox_KeyPress;
            cookingUtensilComboBox.SelectedIndexChanged += CookingUtensilComboBox_SelectedIndexChanged;
            foreach(object item in cookingUtensilComboBox.Items)
            {
                if ((item as string) == cookingUtensil)
                {
                    cookingUtensilComboBox.SelectedIndex = cookingUtensilComboBox.Items.IndexOf(item);
                }
            }
            CheckUseComboBox();
            //
            //  
            //
            temperatureLabel.AutoSize = false;
            temperatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            temperatureLabel.Location = new System.Drawing.Point(cookingUtensilComboBox.Right + recipeDrawSpacing, cookingUtensilComboBox.Top);
            temperatureLabel.Name = "temperatureLabel" + name;
            temperatureLabel.Size = new System.Drawing.Size(110, 20);
            temperatureLabel.Text = "Temperature:";
            //
            //
            //
            temperatureNumeric.Location = new System.Drawing.Point(temperatureLabel.Right + recipeDrawSpacing, cookingUtensilComboBox.Top);
            temperatureNumeric.Name = "numericUpDown" + name;
            temperatureNumeric.Size = new System.Drawing.Size((int)(halfWidthWithSpacing * .5), 20);
            temperatureNumeric.TabIndex = 3;
            temperatureNumeric.Value = temperature;
            temperatureNumeric.Increment = 1;
            temperatureNumeric.Maximum = 1000;
            temperatureNumeric.Minimum = 0;
            temperatureNumeric.Visible = !useComboBox;
            temperatureNumeric.Enabled = !useComboBox;
            //
            //
            //
            temperatureComboBox.Location = new System.Drawing.Point(temperatureLabel.Right + recipeDrawSpacing, cookingUtensilComboBox.Top);
            temperatureComboBox.Name = "temperatureCombo" + name;
            temperatureComboBox.Size = new Size((int)(halfWidthWithSpacing * .5), 20);
            temperatureComboBox.TabIndex = 3;
            temperatureComboBox.Items.AddRange(new string[] { "Low Heat", "Medium Low Heat", "Medium Heat", "Medium High Heat", "High Heat" });
            if (temperature < temperatureComboBox.Items.Count)
            {
                temperatureComboBox.SelectedIndex = temperature;
            }
            temperatureComboBox.Visible = useComboBox;
            temperatureComboBox.Enabled = useComboBox;
            temperatureComboBox.KeyPress += TemperatureComboBox_KeyPress;
            temperatureComboBox.SelectedIndexChanged += TemperatureComboBox_SelectedIndexChanged;
            //
            //
            //
            timeLabel.AutoSize = false;
            timeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            timeLabel.Location = new System.Drawing.Point(cookingUtensilComboBox.Left, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            timeLabel.Name = "timeLabel" + name;
            timeLabel.Size = new System.Drawing.Size(60, 20);
            timeLabel.Text = "Time:";
            //
            //
            //
            hourNumeric.Location = new System.Drawing.Point(timeLabel.Right + recipeDrawSpacing, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            hourNumeric.Name = "hourNumeric" + name;
            hourNumeric.Size = new System.Drawing.Size(40, 20);
            hourNumeric.TabIndex = 4;
            hourNumeric.Value = hours;
            hourNumeric.Increment = 1;
            hourNumeric.Maximum = 72;
            hourNumeric.Minimum = 0;
            //
            //
            //
            hourLabel.AutoSize = false;
            hourLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            hourLabel.Location = new System.Drawing.Point(hourNumeric.Right, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            hourLabel.Name = "hourLabel" + name;
            hourLabel.Size = new System.Drawing.Size(60, 20);
            hourLabel.Text = "hours";
            //
            //
            //
            minuteNumeric.Location = new System.Drawing.Point(hourLabel.Right + recipeDrawSpacing, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            minuteNumeric.Name = "minuteNumeric" + name;
            minuteNumeric.Size = new System.Drawing.Size(60, 20);
            minuteNumeric.TabIndex = 5;
            minuteNumeric.Value = (decimal)minutes;
            minuteNumeric.Increment = .5m;
            minuteNumeric.Maximum = 60;
            minuteNumeric.Minimum = 0;
            minuteNumeric.DecimalPlaces = 1;
            //
            //
            //
            minuteLabel.AutoSize = false;
            minuteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            minuteLabel.Location = new System.Drawing.Point(minuteNumeric.Right, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            minuteLabel.Name = "minuteLabel" + name;
            minuteLabel.Size = new System.Drawing.Size(80, 20);
            minuteLabel.Text = "minutes";
            //
            //
            //
            servingsLabel.AutoSize = false;
            servingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            servingsLabel.Location = new System.Drawing.Point(timeLabel.Left, minuteNumeric.Bottom + recipeDrawSpacing);
            servingsLabel.Name = "servingsLabel" + name;
            servingsLabel.Size = new System.Drawing.Size(80, 20);
            servingsLabel.Text = "Servings:";
            //
            //
            //
            servingsNumeric.Location = new System.Drawing.Point(servingsLabel.Right + recipeDrawSpacing, minuteNumeric.Bottom + recipeDrawSpacing);
            servingsNumeric.Name = "servingsNumeric" + name;
            servingsNumeric.Size = new System.Drawing.Size(60, 20);
            servingsNumeric.TabIndex = 6;
            servingsNumeric.Value = (decimal)servings;
            servingsNumeric.Increment = .5m;
            servingsNumeric.Maximum = 1000;
            servingsNumeric.Minimum = 0;
            servingsNumeric.DecimalPlaces = 1;
            //
            //
            //
            descriptionTextBox.Location = new System.Drawing.Point(recipeDrawSpacing, height - 150);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox" + name;
            descriptionTextBox.Size = new System.Drawing.Size(usableWidth + recipeDrawSpacing, 150);
            descriptionTextBox.TabIndex = 7;
            descriptionTextBox.ApplyWatermark("Description");
            if (description != "")
            {
                descriptionTextBox.RemoveWatermak();
                descriptionTextBox.Text = description;
            }
            description = descriptionTextBox.Text;

            descriptionTextBox.KeyPress += DescriptionTextBox_KeyPress;
            descriptionTextBox.KeyDown += DescriptionTextBox_KeyDown;
            //
            //
            //
            addIngredientButton.Location = new Point(recipeNameTextBox.Left, recipePictureBox.Bottom - 30);
            addIngredientButton.Name = "addIngredientButton" + name;
            addIngredientButton.Text = "Add Ingredient";
            addIngredientButton.Size = new Size((usableWidth / 2 - recipeDrawSpacing) / 2, 30);
            addIngredientButton.TabIndex = 8;
            addIngredientButton.Click += AddIngredientButton_Click;
            //
            //
            //
            removeIngredientButton.Location = new Point(addIngredientButton.Right + recipeDrawSpacing, recipePictureBox.Bottom - 30);
            removeIngredientButton.Name = "addIngredientButton" + name;
            removeIngredientButton.Text = "Remove Ingredient";
            removeIngredientButton.Size = new Size((usableWidth / 2 - recipeDrawSpacing) / 2, 30);
            removeIngredientButton.TabIndex = 8;
            removeIngredientButton.Click += RemoveIngredientButton_Click;
            //
            //
            //
            ingredientPanel.Location = new System.Drawing.Point(recipeDrawSpacing, recipePictureBox.Bottom + recipeDrawSpacing);
            ingredientPanel.Name = "ingredientPanel" + name;
            ingredientPanel.Size = new System.Drawing.Size(usableWidth + recipeDrawSpacing, descriptionTextBox.Top - recipeDrawSpacing * 2 - recipePictureBox.Bottom);
            ingredientPanel.TabIndex = 10;
            //
            //
            //


            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion FormGeneration

            if (addFirstIngredient)
                AddIngredient();

            ((System.ComponentModel.ISupportInitialize)(temperatureNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(hourNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(minuteNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(servingsNumeric)).EndInit();

            recipePanel.ResumeLayout(false);
            recipePanel.PerformLayout();

            this.recipeName = recipeNameTextBox.Text;
            this.hours = (byte)hourNumeric.Value;
            this.minutes = (float)minuteNumeric.Value;
            this.description = descriptionTextBox.Text;
            this.cookingUtensil = cookingUtensilComboBox.Text;
            this.servings = (float)servingsNumeric.Value;
            this.imageLocation = recipePictureBox.ImageLocation;

            if (useNumeric)
            {
                this.temperature = (int)temperatureNumeric.Value;
            }
            else if (useComboBox)
            {
                this.temperature = temperatureComboBox.SelectedIndex;
            }
            else
            {
                this.temperature = 0;
            }

        }

        private void CookingUtensilComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckUseComboBox();
            temperatureNumeric.Visible = temperatureNumeric.Enabled = useNumeric;
            temperatureComboBox.Visible = temperatureComboBox.Enabled = useComboBox;
        }

        private void TemperatureComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            temperature = temperatureComboBox.SelectedIndex;
        }

        private void TemperatureComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void CheckUseComboBox()
        {
            
            //{ "Conventional Oven", "Convection Oven", "Pan", "Pot", "Toaster", "Microwave", "Griddle", "Bowl" }
            switch (cookingUtensilComboBox.SelectedIndex)
            {
                case 0:
                    useNumeric = true;
                    useComboBox = false;
                    temperatureLabel.Visible = true;
                    break;
                case 1:
                    useNumeric = true;
                    useComboBox = false;
                    temperatureLabel.Visible = true;
                    break;
                case 2:
                    useNumeric = false;
                    useComboBox = true;
                    temperatureLabel.Visible = true;
                    break;
                case 3:
                    useNumeric = false;
                    useComboBox = true;
                    temperatureLabel.Visible = true;
                    break;
                case 4:
                    useNumeric = false;
                    useComboBox = true;
                    temperatureLabel.Visible = true;
                    break;
                case 5:
                    useNumeric = false;
                    useComboBox = true;
                    temperatureLabel.Visible = true;
                    break;
                case 6:
                    useNumeric = true;
                    useComboBox = false;
                    temperatureLabel.Visible = true;
                    break;
                case 7:
                    useNumeric = false;
                    useComboBox = false;
                    temperatureLabel.Visible = false;
                    break;
                
                default:
                    useNumeric = true;
                    useComboBox = false;
                    temperatureLabel.Visible = true;
                    break;
            }
        }

        private void DescriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                descriptionTextBox.SelectAll();
        }

        private void RecipePictureBox_DragDrop(object sender, DragEventArgs e)
        {
            MyDebug.Print("DragDropImage");

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if(files != null)
            {
                Image dragDropPic = Image.FromFile(files[0]);
                recipePictureBox.Image = dragDropPic;
                string ext = GetImageFormat(dragDropPic);

                if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MyCookbook\Pictures\"))
                    System.IO.Directory.CreateDirectory((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MyCookbook\Pictures\"));

                string picturePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MyCookbook\Pictures\" + recipeName + ext;

                dragDropPic.Save(picturePath);

                recipePictureBox.ImageLocation = picturePath;

                recipePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            }        
            else
            {
                MyDebug.Warn("Dragged File resulted in null value", "RecipePictureBox_DragDrop");
            }    
        }

        public static string GetImageFormat(System.Drawing.Image img)
        {
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                return ".jpg";
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                return ".bmp";
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                return ".png";
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
                return ".emf";
            //if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
            //    return System.Drawing.Imaging.ImageFormat.Exif;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                return ".gif";
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                return ".ico";
            //if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
            //    return System.Drawing.Imaging.ImageFormat.MemoryBmp;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                return ".tif";
            else
                return ".wmf";
        }


        private void RecipePictureBox_DragEnter(object sender, DragEventArgs e)
        {
            MyDebug.Print("DragEnterImage");
            e.Effect = DragDropEffects.All;
        }


        private void RecipePictureBox_DoubleClick(object sender, EventArgs e)
        {
            string path;

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.RestoreDirectory = true;
            openFile.DefaultExt = "png";
            openFile.Filter = "PNG Images|*.png|JPEG Images|*.jpg|GIF Images|*.gih|BITMAPS|*.bmp";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                path = openFile.FileName;
                try
                {
                    recipePictureBox.Image = Image.FromFile(path);
                    recipePictureBox.ImageLocation = path;
                    this.imageLocation = path;
                    MyDebug.Print(imageLocation);
                    recipePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                }
                catch (Exception)
                {
                    MyDebug.Error("Image Convert failed for file: " + path, "RecipePictureBox_DoubleClick", 373);
                }
            }
        }

        private void RemoveIngredientButton_Click(object sender, EventArgs e)
        {
            ingredientList.Last().ingredientPanel.Dispose();
            ingredientList.Remove(ingredientList.Last());
        }

        private void DescriptionTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            EventControls.ValidateKeyPress(e, descriptionTextBox.Text, descriptionTextBox.SelectionStart);
        }

        private void CookingUtensilComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            EventControls.IgnoreKeyPress(e, '"');
        }

        private void RecipeNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            EventControls.IgnoreKeyPress(e, '"');
        }

        public Recipe(string name, TreeNode node, int width, int height, int x, int y) : this(name, node, width, height, x, y, 0, 0, 0, "", "", 0, "", true) { }


        private void RecipeNameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.node.Name = this.node.Text = this.recipeName = recipeNameTextBox.Text;
        }

        private void AddIngredientButton_Click(object sender, EventArgs e)
        {
            AddIngredient();
        }

        public void SetButtonVisible(bool isVisible)
        {
            addIngredientButton.Visible = isVisible;
            removeIngredientButton.Visible = isVisible;
        }

        public void AddIngredient(string name, string amount, string unitType)
        {
            int x = 1,
                y = 1,
                count = ingredientList.Count;

            if (ingredientList.Count != 0)
            {
                Panel lastIngredientPnl = ingredientList.Last().ingredientPanel;
                if (count % 2 == 1)
                {
                    y += lastIngredientPnl.Top;
                }
                else
                {
                    y += lastIngredientPnl.Bottom + recipeDrawSpacing;
                }
            }

            if (count % 2 == 1)
            {   //Add to right side
                x += (ingredientPanel.Width + recipeDrawSpacing) / 2;

            }

            Ingredient.width = (recipePanel.Width - 3 * recipeDrawSpacing)/2 - 1;            
            ingredientList.Add(new Ingredient(x, y, name, amount, unitType));
            ingredientPanel.Controls.Add(ingredientList.Last().ingredientPanel);
            ingredientList.Last().unitComboBox.KeyDown += UnitComboBox_KeyDown;

            if (ingredientList.Count > 16)
            {
                ResizeRecipe(recipePanel.Width, recipePanel.Height);
            }

        }

        
        public void AddIngredient()
        {
            AddIngredient("Ingredient Name", "Amt.", "");
        }

        private void UnitComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if(ingredientList.Last().unitComboBox.Equals(sender))
                {
                    AddIngredient();
                }                
            }
        }

        public void SetSize(int width, int height)
        {
            ingredientPanel.Size = new System.Drawing.Size(width, height);
        }

        public void SetVisible(bool isVisible)
        {
            this.recipePanel.Visible = isVisible;
            this.recipePanel.Enabled = isVisible;
        }

        public void UpdateVariables()
        {
            if( this.recipeName != recipeNameTextBox.Text ||
                this.hours != (byte)hourNumeric.Value ||
                this.minutes != (float)minuteNumeric.Value ||
                this.description != descriptionTextBox.Text ||
                this.cookingUtensil != cookingUtensilComboBox.Text ||
                this.servings != (float)servingsNumeric.Value ||
                this.imageLocation != recipePictureBox.ImageLocation ||
                (useNumeric && this.temperature != (int)temperatureNumeric.Value) ||
                (useComboBox && this.temperature != temperatureComboBox.SelectedIndex)
            )
            {
                this.recipeName = recipeNameTextBox.Text;                
                this.hours = (byte)hourNumeric.Value;
                this.minutes = (float)minuteNumeric.Value;
                this.description = descriptionTextBox.Text;
                this.cookingUtensil = cookingUtensilComboBox.Text;
                this.servings = (float)servingsNumeric.Value;
                this.imageLocation = recipePictureBox.ImageLocation;

                if (useNumeric)
                {
                    this.temperature = (int)temperatureNumeric.Value;
                }
                else if (useComboBox)
                {
                    this.temperature = temperatureComboBox.SelectedIndex;
                }
                else
                {
                    this.temperature = 0;
                }

                ((MainForm)recipePanel.Parent).needSave = true;
                
            }
            

        }

        public void ResizeRecipe(int width, int height)
        {
            if (recipePanel.VerticalScroll.Visible == true)
            {
                usableWidth = width - 3 * recipeDrawSpacing - scrollBarWidth;
            }
            else
            {
                usableWidth = width - 3 * recipeDrawSpacing;
            }

            recipePanel.Size = new System.Drawing.Size(width, height);

            recipePictureBox.Location = new System.Drawing.Point(recipeDrawSpacing, recipeDrawSpacing);
            recipePictureBox.Size = new System.Drawing.Size(usableWidth / 2, 200);

            recipeNameTextBox.Location = new System.Drawing.Point(recipePictureBox.Right + recipeDrawSpacing, recipeDrawSpacing);
            recipeNameTextBox.Size = new System.Drawing.Size(usableWidth / 2, 44);

            //Used to divide the right half of the panel into 3 parts           70 is the temperature label width
            int halfWidthWithSpacing = (usableWidth / 2) - 2 * recipeDrawSpacing - 110;

            cookingUtensilComboBox.Location = new System.Drawing.Point(recipeNameTextBox.Left, recipeNameTextBox.Bottom + recipeDrawSpacing);
            cookingUtensilComboBox.Size = new System.Drawing.Size((int)(halfWidthWithSpacing * .5), 21);

            temperatureLabel.Location = new System.Drawing.Point(cookingUtensilComboBox.Right + recipeDrawSpacing, cookingUtensilComboBox.Top);
            temperatureLabel.Size = new System.Drawing.Size(110, 20);

            temperatureNumeric.Location = new System.Drawing.Point(temperatureLabel.Right + recipeDrawSpacing, cookingUtensilComboBox.Top);
            temperatureNumeric.Size = new System.Drawing.Size((int)(halfWidthWithSpacing * .5), 20);

            temperatureComboBox.Location = new System.Drawing.Point(temperatureLabel.Right + recipeDrawSpacing, cookingUtensilComboBox.Top);
            temperatureComboBox.Size = new Size((int)(halfWidthWithSpacing * .5), 20);


            timeLabel.Location = new System.Drawing.Point(cookingUtensilComboBox.Left, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            timeLabel.Size = new System.Drawing.Size(60, 20);

            hourNumeric.Location = new System.Drawing.Point(timeLabel.Right + recipeDrawSpacing, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            hourNumeric.Size = new System.Drawing.Size(40, 20);


            hourLabel.Location = new System.Drawing.Point(hourNumeric.Right, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            hourLabel.Size = new System.Drawing.Size(60, 20);

            minuteNumeric.Location = new System.Drawing.Point(hourLabel.Right + recipeDrawSpacing, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            minuteNumeric.Size = new System.Drawing.Size(60, 20);

            minuteLabel.Location = new System.Drawing.Point(minuteNumeric.Right, cookingUtensilComboBox.Bottom + recipeDrawSpacing);
            minuteLabel.Size = new System.Drawing.Size(80, 20);

            servingsLabel.Location = new System.Drawing.Point(timeLabel.Left, minuteNumeric.Bottom + recipeDrawSpacing);
            servingsLabel.Size = new System.Drawing.Size(80, 20);

            servingsNumeric.Location = new System.Drawing.Point(servingsLabel.Right + recipeDrawSpacing, minuteNumeric.Bottom + recipeDrawSpacing);
            servingsNumeric.Size = new System.Drawing.Size(60, 20);

            addIngredientButton.Location = new Point(recipeNameTextBox.Left, recipePictureBox.Bottom - 30);
            addIngredientButton.Size = new Size((usableWidth / 2 - recipeDrawSpacing) / 2, 30);
            //
            //
            //
            removeIngredientButton.Location = new Point(addIngredientButton.Right + recipeDrawSpacing, recipePictureBox.Bottom - 30);
            removeIngredientButton.Size = new Size((usableWidth / 2 - recipeDrawSpacing) / 2, 30);


            descriptionTextBox.Size = new System.Drawing.Size(usableWidth + recipeDrawSpacing, 150);

            ingredientPanel.Location = new System.Drawing.Point(recipeDrawSpacing, recipePictureBox.Bottom + recipeDrawSpacing);
            ingredientPanel.Size = new System.Drawing.Size(usableWidth + recipeDrawSpacing, descriptionTextBox.Top - recipeDrawSpacing * 2 - recipePictureBox.Bottom);


            if (ingredientList.Count != 0)
            {
                int count = 0;

                foreach (Ingredient ingred in ingredientList)
                {
                    int x = 1, y = 1;
                    if (count > 0)
                    {
                        Panel ingredientPnl = ingredientList[count - 1].ingredientPanel;
                        if (count % 2 == 1)
                        {
                            y += ingredientPnl.Top;
                        }
                        else
                        {
                            y += ingredientPnl.Bottom + recipeDrawSpacing;
                        }
                    }

                    if (count % 2 == 1)
                    {   //Add to right side
                        x += (ingredientPanel.Width + recipeDrawSpacing) / 2;
                    }


                    Ingredient.width = recipePictureBox.Width - 1;
                    ingred.IngredientResize(x, y);
                    count++;
                }
                Panel lastIngredientPanel = ingredientList.Last().ingredientPanel;
                if (ingredientList.Count > 16)
                {
                    ingredientPanel.Height = lastIngredientPanel.Bottom + recipeDrawSpacing;
                    descriptionTextBox.Location = new System.Drawing.Point(recipeDrawSpacing, ingredientPanel.Bottom);

                }
            }


        }
        

        public byte[] ToByteArray()
        {
            UpdateVariables();
            string seperator = @""",""";

            string accumulatedIngredients = String.Empty;

            foreach (Ingredient ing in ingredientList)
            {
                accumulatedIngredients += "," + ing.ToString();
            }

            return GetBytes(",\n\"" +
                            this.recipeName + seperator +
                            this.temperature + seperator +
                            this.hours + seperator +
                            this.minutes + seperator +
                            this.description + seperator +
                            this.cookingUtensil + seperator +
                            this.servings + seperator +
                            this.imageLocation + seperator +
                            ingredientList.Count + @"""" +
                            accumulatedIngredients);


        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static void ResetIndex()
        {
            indexLooper = 0;
        }

        public void ReloadRecipeIndex()
        {
            this.index = indexLooper;
            indexLooper++;
        }

        public override string ToString()
        {
            return this.recipeName;
        }

    }
}
