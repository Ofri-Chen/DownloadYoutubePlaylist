using System;
using System.Configuration;

namespace DownloadYoutubePlaylist
{
    public static class ConfigManager
    {
        public static string DownloadFolderPath = ConfigurationManager.AppSettings["downloadFolderPath"];

        public static string BaseTargetDirectoryPath = ConfigurationManager.AppSettings["targetFolderPath"];

        public static string ChromeDriverPath = ConfigurationManager.AppSettings["chromeDriverPath"];

        public static string LogFileName = ConfigurationManager.AppSettings["logFileName"];

        public static int WaitForConversion = Convert.ToInt32(ConfigurationManager.AppSettings["waitForConversion"]);

        public static int WaitTillDownloadIsFinished = Convert.ToInt32(ConfigurationManager.AppSettings["waitTillDownloadIsFinished"]);
        
        public static int DefaultLimit = Convert.ToInt32(ConfigurationManager.AppSettings["defaultLimit"]);

        #region API Configs

        public static string APIBaseUrl = ConfigurationManager.AppSettings["apiBaseUrl"];

        public static string APIGetTopTracks = ConfigurationManager.AppSettings["apiGetTopTracks"];

        public static string APIAuthorizationKey = ConfigurationManager.AppSettings["apiAuthorizationKey"];

        #endregion
    }
}