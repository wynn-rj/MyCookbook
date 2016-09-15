namespace MyCookbook
{
    partial class SearchBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchBarListBox = new System.Windows.Forms.ListBox();
            this.searchBarTextBox = new RJWynn.Forms.WatermarkTextBox();
            this.searchBarButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // searchBarListBox
            // 
            this.searchBarListBox.BackColor = System.Drawing.SystemColors.Menu;
            this.searchBarListBox.FormattingEnabled = true;
            this.searchBarListBox.Location = new System.Drawing.Point(0, 20);
            this.searchBarListBox.Name = "searchBarListBox";
            this.searchBarListBox.Size = new System.Drawing.Size(170, 17);
            this.searchBarListBox.Sorted = true;
            this.searchBarListBox.TabIndex = 2;
            this.searchBarListBox.Visible = false;
            // 
            // searchBarTextBox
            // 
            this.searchBarTextBox.ForeColor = System.Drawing.Color.Gray;
            this.searchBarTextBox.Location = new System.Drawing.Point(0, 0);
            this.searchBarTextBox.Name = "searchBarTextBox";
            this.searchBarTextBox.Size = new System.Drawing.Size(150, 20);
            this.searchBarTextBox.TabIndex = 3;
            this.searchBarTextBox.Text = "Type here";
            this.searchBarTextBox.WatermarkActive = true;
            this.searchBarTextBox.WatermarkText = "Search Recipes";
            this.searchBarTextBox.TextChanged += new System.EventHandler(this.searchBarTextBox_TextChanged);
            // 
            // searchBarButton
            // 
            this.searchBarButton.BackColor = System.Drawing.SystemColors.Window;
            this.searchBarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchBarButton.ForeColor = System.Drawing.SystemColors.Window;
            this.searchBarButton.Image = global::MyCookbook.Properties.Resources.SearchMagnifyingGlassIcon;
            this.searchBarButton.Location = new System.Drawing.Point(150, 0);
            this.searchBarButton.Margin = new System.Windows.Forms.Padding(0);
            this.searchBarButton.Name = "searchBarButton";
            this.searchBarButton.Size = new System.Drawing.Size(20, 20);
            this.searchBarButton.TabIndex = 0;
            this.searchBarButton.UseVisualStyleBackColor = false;
            // 
            // SearchBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.searchBarListBox);
            this.Controls.Add(this.searchBarTextBox);
            this.Controls.Add(this.searchBarButton);
            this.Name = "SearchBar";
            this.Size = new System.Drawing.Size(170, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        public System.Windows.Forms.ListBox searchBarListBox;
        public RJWynn.Forms.WatermarkTextBox searchBarTextBox;
        public System.Windows.Forms.Button searchBarButton;
    }
}
