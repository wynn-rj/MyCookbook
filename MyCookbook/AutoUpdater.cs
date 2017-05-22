using RJWynn.Debugging;
using System;
using System.IO;
using System.Net;

namespace MyCookbook
{
    class AutoUpdater
    {
        static string PATH = @"https://api.github.com/repos/wynn-rj/MyCookbook/releases/latest";
        static string DOWNLOAD_PATH_TOP = @"https://github.com/wynn-rj/MyCookbook/releases/download/";
        static string DOWNLOAD_PATH_END = @"/MyCookbookSetup.msi";

        /// <summary>
        /// Checks the GitHub repository to see if a new version of the program is out.
        /// </summary>
        /// <returns>The latest released version</returns>
        public static string CheckForUpdate()
        {
            MyDebug.Print("Checking for new version");
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PATH);
                //webRequest.CookieContainer = new CookieContainer();
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.KeepAlive = false;
                webRequest.Method = "GET";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Accept = "text";
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.168 Safari/535.19";

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());

                string html = streamReader.ReadToEnd();
                webResponse.Close();

                int tagIndex = html.IndexOf("tag_name");
                string version = "";
                if (tagIndex >= 0)
                {
                    tagIndex += 12;
                    int endQuote = html.IndexOf("\"", tagIndex);

                    version = html.Substring(tagIndex, endQuote - tagIndex);
                }

                return version;
            }
            catch (Exception e)
            {
                MyDebug.Print("Error occured while checking for update");
                MyDebug.Error(e);
                throw e;
            }
            
        }

        /// <summary>
        /// Downloads the specified <code>version</code> of the installer from GitHub.
        /// </summary>
        /// <param name="version">Version ID for version to download</param>
        public static void GetUpdate(string version)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MyCookbook\MyCookbookSetup.msi";
            MyDebug.Print("Downloading new version");
            try
            {
                WebClient Client = new WebClient();
                Client.DownloadFile(DOWNLOAD_PATH_TOP + version + DOWNLOAD_PATH_END, filePath);
            }
            catch (Exception e)
            {
                MyDebug.Print("Error occured while getting update");
                MyDebug.Error(e);
                throw e;
            }
                 
        }

        /// <summary>
        /// Compares to version IDs to see which one is newer.
        /// </summary>
        /// <param name="oldVersion"></param>
        /// <param name="newVersion"></param>
        /// <returns></returns>
        public static bool CheckVersions(string oldVersion, string newVersion)
        {
            

            //Removes v in string
            newVersion = newVersion.Substring(1);
            MyDebug.Print("Comparing Versions - OV: " + oldVersion + " - NV: " + newVersion);

            string[] oldVersionSplit = oldVersion.Split('.');
            string[] newVersionSplit = newVersion.Split('.');

            bool isSameOrNewer = true;
            for(int i = 0; i < oldVersionSplit.Length; i++)
            {
                if(isSameOrNewer)
                {
                    try
                    {
                        int oldInt = Int32.Parse(oldVersionSplit[i]);
                        int newInt = Int32.Parse(newVersionSplit[i]);

                        isSameOrNewer = (oldInt >= newInt);
                    }
                    catch (Exception)
                    {
                        MyDebug.Error("Versions were unable to be converted to ints at " + i, "AutoUpdater.CheckVersions", 65);
                        return true;
                    }
                }
            }

            return !isSameOrNewer;
        }
    }

    
}
