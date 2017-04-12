using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading;
using System.Configuration;

namespace DownloadYoutubePlaylist
{
    public static class ConfigManager
    {
        public static string DownloadFolderPath = ConfigurationManager.AppSettings["downloadFolderPath"];

        public static string BaseTargetDirectoryPath = ConfigurationManager.AppSettings["targetFolderPath"];

        public static string ChromeDriverPath = ConfigurationManager.AppSettings["chromeDriverPath"];

        public static string LogFileName = ConfigurationManager.AppSettings["logFileName"];

        public static int Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["timeout"]);

        public static int WaitForConversion = Convert.ToInt32(ConfigurationManager.AppSettings["waitForConversion"]);
    }
}
