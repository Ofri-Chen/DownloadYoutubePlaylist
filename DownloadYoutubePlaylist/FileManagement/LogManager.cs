using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DownloadYoutubePlaylist.FileManagement
{
    public static class LogManager
    {
        //public static void LogSucess(string title)
        //{
        //    Log("Sucess", title);
        //}

        //public static void LogException(string message)
        //{
        //    Log("Exception", message);
        //}
        public static void Log(string logInfo, bool isSucess)
        {
            WriteLog(((isSucess) ? "Sucess" : "Failure"), logInfo);
        }

        private static void WriteLog(string status, string logInfo)
        {
            StreamWriter file = File.AppendText(Resources.TargetDirectory + "\\" + ConfigManager.LogFileName);
            file.WriteLine("[" + DateTime.Now + "]  -  " + status + ": " + logInfo);
            file.Close();
        }
    }
}
