using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RJWynn.Debugging
{
    

    public static class MyDebug
    {
        private static bool doLog = false;
        private static string file = null;
        private static string errorFile = null;

        #region Constructors
        /// <summary>
        /// Initializes a debug interface. Allows the choice of whether or not it writes to a log file, and what that file is called.
        /// </summary>
        /// <param name="doLog"></param>
        /// <param name="file"></param>
        public static void Initialize(bool doLog, string file)
        {            
            MyDebug.errorFile = file + @"\ErrorLog.txt";
            MyDebug.doLog = doLog;

            file += @"\log.txt";
            MyDebug.file = file;
            
            if (doLog)
            {
                if(File.Exists(file))
                {
                    DateTime LastEdit = File.GetLastWriteTime(file);
                    
                    if(LastEdit.Month == DateTime.Now.Month)
                    {
                        using (StreamWriter w = File.AppendText(file))
                        {
                            w.WriteLine(String.Format("Debugger Started on {0}", System.DateTime.Now.ToLocalTime().ToString()));
                        }
                    }
                    else
                    {
                            File.WriteAllText(file, String.Format("Debugger Started on {0}\n", System.DateTime.Now.ToLocalTime().ToString()));
                    }
                    
                }
                else
                {
                    File.WriteAllText(file, String.Format("Debugger Started on {0}\n", System.DateTime.Now.ToLocalTime().ToString()));
                }
                
                        
            }
        }

        /// <summary>
        /// Initializes a debug interface with no log file.
        /// </summary>
        public static void Initialize()
        {
            MyDebug.file = "";
        }


        #endregion Constructors

            #region Debugging methods
            /// <summary>
            /// Prints debug comment with header WARN: and the method the warning occured
            /// </summary>
            /// <param name="message"></param>
            /// <param name="currentMethod"></param>
        public static void Warn(string message, string currentMethod)
        {
            WriteToLog(String.Format("WARN: \"{0}\" @code: {1}", message, currentMethod));
        }

        /// <summary>
        /// Prints debug comment with header ERROR: and the method the error occured and the line it occured at
        /// </summary>
        /// <param name="message"></param>
        /// <param name="currentMethod"></param>
        /// <param name="lineNumberOfClass"></param>
        public static void Error(string message, string currentMethod, uint lineNumberOfClass)
        {
            WriteToLog(String.Format("ERROR: \"{0}\" @code: {1}, line: {2}", message, currentMethod, lineNumberOfClass));
            if (errorFile != null)
            {
                if (doLog)
                {
                    using (StreamWriter w = File.AppendText(errorFile))
                    {
                        w.WriteLine(String.Format("ERROR: \"{0}\" @code: {1}, line: {2}", message, currentMethod, lineNumberOfClass));
                    }
                }
            }
        }

        /// <summary>
        /// Prints debug comment with header ERROR: and the method the error occured and the line it occured at
        /// </summary>
        /// <param name="message"></param>
        /// <param name="currentMethod"></param>
        /// <param name="lineNumberOfClass"></param>
        public static void Error(Exception exception)
        {
            WriteToLog(String.Format("ERROR: \"{0}\" @code: {1}, source: {2}", exception.Message, exception.TargetSite, exception.Source));
            if (errorFile != null)
            {
                if (doLog)
                {
                    using (StreamWriter w = File.AppendText(errorFile))
                    {
                        w.WriteLine(String.Format("ERROR: \"{0}\" @code: {1}, source: {2}", exception.Message, exception.TargetSite, exception.Source));
                    }
                }
            }
        }

        /// <summary>
        /// Prints debug comment with header Debug: and the method the message occured
        /// </summary>
        /// <param name="message"></param>
        /// <param name="currentMethod"></param>
        public static void Print(string message, string currentMethod)
        {
            WriteToLog(String.Format("Debug: \"{0}\" @code: {1}", message, currentMethod));
            
        }

        /// <summary>
        /// Prints debug comment with header Debug:
        /// </summary>
        /// <param name="message"></param>
        public static void Print(string message)
        {
            WriteToLog(String.Format("Debug: \"{0}\"", message));
        }
        #endregion Debugging methods

        private static void WriteToLog(string logMessage)
        {
            Debug.WriteLine(logMessage);
            if (file != null)
            {
                if (doLog)
                {
                    using (StreamWriter w = File.AppendText(file))
                    {
                        w.WriteLine(logMessage);
                    }
                }
            } 
            else
            {
                throw new Exception("The MyDebug Class must use the Initializer function before any other methods are used");
            }
        }

    }
}
