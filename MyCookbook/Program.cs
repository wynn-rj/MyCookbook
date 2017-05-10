using System;
using System.Windows.Forms;

namespace MyCookbook
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                if(!e.Message.Equals("Cannot access a disposed object.\r\nObject name: 'MainForm'."))
                {
                    throw;
                }                
            }
        }
    }
}
