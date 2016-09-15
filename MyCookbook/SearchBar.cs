using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RJWynn.Debugging;

namespace MyCookbook
{
    public partial class SearchBar : UserControl
    {
        public string[] Items { get; private set; }

        public object Selected {   get { return searchBarListBox.SelectedItem; }
                            set { searchBarListBox.SelectedItem = value; }
                        }
        
        public event EventHandler OnSelection;

        private bool _UseSearchButton;
        private string text;

        public bool UseSearchButton
        {
            get
            {
                return _UseSearchButton;
            }

            set
            {
                _UseSearchButton = value;
                if (value)
                {
                    searchBarButton.Visible = true;
                    searchBarButton.Enabled = true;
                    searchBarTextBox.Width = this.Width - 20;
                    searchBarButton.Location = new Point(searchBarTextBox.Right, searchBarTextBox.Top);
                }
                else
                {
                    searchBarButton.Visible = false;
                    searchBarButton.Enabled = false;
                    searchBarTextBox.Width = this.Width;
                }
            }
        }

        public SearchBar()
        {
            InitializeComponent();
            Items = new string[0];
            searchBarListBox.VisibleChanged += SearchBarListBox_VisibleChanged;
            searchBarTextBox.KeyDown += SearchBarTextBox_KeyDown;
            searchBarListBox.Click += SearchBarListBox_Click;
        }

        private void SearchBarListBox_Click(object sender, EventArgs e)
        {
            if (OnSelection != null)
                OnSelection(this, null);
        }

        private void SearchBarTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                if(searchBarListBox.SelectedIndex > 0)
                {
                    searchBarListBox.SelectedIndex -= 1;
                }
            }
            else if(e.KeyCode == Keys.Down)
            {
                if(searchBarListBox.SelectedIndex < searchBarListBox.Items.Count - 1)
                {
                    searchBarListBox.SelectedIndex += 1;
                }
            }
            else if(e.KeyCode == Keys.Enter)
            {
                if(searchBarListBox.SelectedIndex != -1)
                {
                    if (OnSelection != null)
                        OnSelection(this, null);
                }
            }

        }

        private void SearchBarListBox_VisibleChanged(object sender, EventArgs e)
        {
            if(searchBarListBox.Visible)
            {
                searchBarListBox.Height = 0;
                this.Height = searchBarTextBox.Bottom;

            }
        }

        public void AddItems(string[] Items)
        {
            List<string> toArray = new List<string>(this.Items);
            toArray.AddRange(Items);
            this.Items = toArray.ToArray();
        }

        public void AddItem(string Item)
        {
            List<string> toArray = new List<string>(Items);
            toArray.Add(Item);
            Items = toArray.ToArray();
        }

        public void ClearItems()
        {
            ClearItems(true);
        }

        public void ClearItems(bool clearText)
        {
            Items = new string[0];
            searchBarListBox.Items.Clear();
            searchBarListBox.Visible = false;
            this.Height = searchBarTextBox.Bottom;
            if (clearText)
            {
                searchBarTextBox.Text = "";
            }          

        }

        private void searchBarTextBox_TextChanged(object sender, EventArgs e)
        {
            text = searchBarTextBox.Text;
            if(searchBarTextBox.Text != "" && searchBarTextBox.WatermarkActive == false)
            {                
                if (Items.Length > 0)
                {
                    List<string> toDisplay = new List<string>();

                    foreach (string str in Items)
                    {
                        if (str.ToLower().Contains(searchBarTextBox.Text.ToLower()))
                            toDisplay.Add(str);
                    }

                    MyDebug.Print("Search found total of " + (toDisplay.Count) + " items");
                    foreach (string str in toDisplay)
                    {
                        MyDebug.Print("Search found: " + str);
                    }

                    searchBarListBox.Items.Clear();
                    searchBarListBox.Items.AddRange(toDisplay.ToArray());
                    searchBarListBox.Visible = true;
                    searchBarListBox.Enabled = true;
                    searchBarListBox.Height = (searchBarListBox.Items.Count + 1) * searchBarListBox.ItemHeight;
                    searchBarListBox.BringToFront();
                    this.Height = searchBarListBox.Bottom;

                    if (searchBarListBox.Items.Count == 0)
                    {
                        searchBarListBox.Visible = false;
                        this.Height = searchBarTextBox.Bottom;
                    }

                    
                }
                
            }
            else
            {
                searchBarListBox.Visible = false;

                this.Height = searchBarTextBox.Bottom;
            }
            
        }


        public void SearchBar_Resize(object sender, System.EventArgs e)
        {
            if(_UseSearchButton)
            {
                searchBarTextBox.Width = this.Width - 20;
                searchBarButton.Location = new Point(searchBarTextBox.Right, searchBarTextBox.Top);
                searchBarListBox.Width = this.Width;
            }
            else
            {
                searchBarTextBox.Width = this.Width;
                searchBarListBox.Width = this.Width;
            }
            searchBarTextBox.Text = text;
        }

        
    }
}
