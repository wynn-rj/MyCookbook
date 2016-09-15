using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RJWynn.Forms
{
    static class EventControls
    {
        /// <summary>
        /// Called by key press events. Prevents the user from inputing a semicolon into a text field. This would create an error from <c>OpenCookbook</c>        
        /// </summary>
        /// <param name="e"><c>KeyPressEventArgs</c> from Event</param>
        public static void ValidateKeyPress(KeyPressEventArgs e, string controlText, int selectionStart)
        {
            bool inQuote;
            //Debugging.MyDebug debugger = new Debugging.MyDebug(false);

            string str = controlText.Substring(0, selectionStart);

            //debugger.Print(str);

            int open = str.LastIndexOf((char)0x201C);
            int close = str.LastIndexOf((char)0x201D);

            if (open > close)
            {
                inQuote = true;
            }
            else
            {
                inQuote = false;
            }

            //debugger.Print(((char)148).ToString());
            if (e.KeyChar == '"')
            {
                //debugger.Print("Passed");
                if (inQuote)
                {
                    e.KeyChar = (char)0x201D;     //Close Quote   not "
                }
                else
                {
                    e.KeyChar = (char)0x201C;       //Open Quote    not "
                }
                inQuote = !inQuote;
            }

        }

        public static void IgnoreKeyPress(KeyPressEventArgs e, char charToIgnore)
        {
            if (e.KeyChar == charToIgnore)
            {
                e.Handled = true;
                string message = String.Format("The {0} character is not allowed in this context", charToIgnore);
                string caption = "Invalid character";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                //DialogResult result;

                // Displays the MessageBox.

                MessageBox.Show(message, caption, buttons);
            }
        }
    }
}
