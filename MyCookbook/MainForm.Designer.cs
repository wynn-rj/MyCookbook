using System;

namespace MyCookbook
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripHeader = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownFileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cookbookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recipeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printRecipeToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printCookbookToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printShoppingListToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripInfoButton = new System.Windows.Forms.ToolStripButton();
            this.cookbookTree = new System.Windows.Forms.TreeView();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.searchBar1 = new MyCookbook.SearchBar();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripHeader
            // 
            this.toolStripHeader.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripHeader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownFileButton,
            this.toolStripInfoButton});
            this.toolStripHeader.Location = new System.Drawing.Point(0, 0);
            this.toolStripHeader.Name = "toolStripHeader";
            this.toolStripHeader.Size = new System.Drawing.Size(1064, 25);
            this.toolStripHeader.TabIndex = 0;
            this.toolStripHeader.Text = "toolStrip1";
            // 
            // toolStripDropDownFileButton
            // 
            this.toolStripDropDownFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownFileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.importToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.printToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolStripDropDownFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownFileButton.Name = "toolStripDropDownFileButton";
            this.toolStripDropDownFileButton.Size = new System.Drawing.Size(38, 22);
            this.toolStripDropDownFileButton.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cookbookToolStripMenuItem,
            this.typeToolStripMenuItem,
            this.recipeToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // cookbookToolStripMenuItem
            // 
            this.cookbookToolStripMenuItem.Name = "cookbookToolStripMenuItem";
            this.cookbookToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.cookbookToolStripMenuItem.Text = "Cookbook";
            this.cookbookToolStripMenuItem.Click += new System.EventHandler(this.cookbookToolStripMenuItem_Click);
            // 
            // typeToolStripMenuItem
            // 
            this.typeToolStripMenuItem.Name = "typeToolStripMenuItem";
            this.typeToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.typeToolStripMenuItem.Text = "Type";
            this.typeToolStripMenuItem.Click += new System.EventHandler(this.typeToolStripMenuItem_Click);
            // 
            // recipeToolStripMenuItem
            // 
            this.recipeToolStripMenuItem.Name = "recipeToolStripMenuItem";
            this.recipeToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.recipeToolStripMenuItem.Text = "Recipe";
            this.recipeToolStripMenuItem.Click += new System.EventHandler(this.recipeToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printRecipeToolStripItem,
            this.printCookbookToolStripItem,
            this.printShoppingListToolStripItem});
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.printToolStripMenuItem.Text = "Print";
            // 
            // printRecipeToolStripItem
            // 
            this.printRecipeToolStripItem.Name = "printRecipeToolStripItem";
            this.printRecipeToolStripItem.Size = new System.Drawing.Size(152, 22);
            this.printRecipeToolStripItem.Text = "Recipe";
            this.printRecipeToolStripItem.Click += new System.EventHandler(this.printRecipeToolStripItem_Click);
            // 
            // printCookbookToolStripItem
            // 
            this.printCookbookToolStripItem.Name = "printCookbookToolStripItem";
            this.printCookbookToolStripItem.Size = new System.Drawing.Size(152, 22);
            this.printCookbookToolStripItem.Text = "Cookbook";
            this.printCookbookToolStripItem.Click += new System.EventHandler(this.printCookbookToolStripItem_Click);
            // 
            // printShoppingListToolStripItem
            // 
            this.printShoppingListToolStripItem.Name = "printShoppingListToolStripItem";
            this.printShoppingListToolStripItem.Size = new System.Drawing.Size(152, 22);
            this.printShoppingListToolStripItem.Text = "Shopping List";
            this.printShoppingListToolStripItem.Click += new System.EventHandler(this.printShoppingListToolStripItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripInfoButton
            // 
            this.toolStripInfoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripInfoButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripInfoButton.Image")));
            this.toolStripInfoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripInfoButton.Name = "toolStripInfoButton";
            this.toolStripInfoButton.Size = new System.Drawing.Size(32, 22);
            this.toolStripInfoButton.Text = "Info";
            this.toolStripInfoButton.Click += new System.EventHandler(this.toolStripInfoButton_Click);
            // 
            // cookbookTree
            // 
            this.cookbookTree.BackColor = System.Drawing.SystemColors.Control;
            this.cookbookTree.LabelEdit = true;
            this.cookbookTree.Location = new System.Drawing.Point(0, 25);
            this.cookbookTree.Name = "cookbookTree";
            this.cookbookTree.Size = new System.Drawing.Size(189, 655);
            this.cookbookTree.TabIndex = 1;
            this.cookbookTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.cookbookTree_AfterLabelEdit);
            this.cookbookTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.cookbookTree_NodeMouseClick);
            this.cookbookTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.cookbookTree_NodeMouseDoubleClick);
            this.cookbookTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cookbookTree_KeyDown);
            this.cookbookTree.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cookbookTree_KeyPress);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "ckbk";
            this.saveFileDialog.FileName = "MyCookbook";
            this.saveFileDialog.Filter = "Cookbook files (.ckbk)|*.ckbk|Csv file (.csv)|*.csv";
            this.saveFileDialog.InitialDirectory = "C:\\Users\\ROBERT\\Documents\\MyCookbook\\Cookbooks";
            this.saveFileDialog.RestoreDirectory = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "ckbk";
            this.openFileDialog.FileName = "MyCookbook";
            this.openFileDialog.Filter = "Cookbook files (.ckbk)|*.ckbk";
            this.openFileDialog.InitialDirectory = "C:\\Users\\ROBERT\\Documents\\MyCookbook\\Cookbooks";
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // searchBar1
            // 
            this.searchBar1.Location = new System.Drawing.Point(890, 0);
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.Selected = null;
            this.searchBar1.Size = new System.Drawing.Size(174, 20);
            this.searchBar1.TabIndex = 2;
            this.searchBar1.UseSearchButton = false;
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1064, 681);
            this.Controls.Add(this.searchBar1);
            this.Controls.Add(this.cookbookTree);
            this.Controls.Add(this.toolStripHeader);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1080, 720);
            this.Name = "MainForm";
            this.Text = "MyCookbook";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.toolStripHeader.ResumeLayout(false);
            this.toolStripHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripHeader;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownFileButton;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cookbookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem typeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recipeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripInfoButton;
        private System.Windows.Forms.TreeView cookbookTree;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printRecipeToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem printCookbookToolStripItem;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private SearchBar searchBar1;
        private System.Windows.Forms.ToolStripMenuItem printShoppingListToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    }
}

