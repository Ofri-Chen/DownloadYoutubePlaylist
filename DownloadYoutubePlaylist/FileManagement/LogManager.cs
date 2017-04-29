using System;
using System.IO;

namespace DownloadYoutubePlaylist.FileManagement
{
    public static class LogManager
    {
        public static void Log(string logInfo, bool isSucess)
        {
            WriteLog(((isSucess) ? "Sucess" : "Failure"), logInfo);
        }

        public static void LogFinishWork(int timeInMilliseconds)
        {
            WriteLog("Finished: ", timeInMilliseconds);
        }

        private static void WriteLog(string status, string logInfo)
        {
            StreamWriter file = File.AppendText(Resources.TargetDirectory + "\\" + ConfigManager.LogFileName);
            file.WriteLine("[" + DateTime.Now + "]  -  " + status + ": " + logInfo);
            Console.WriteLine("[" + DateTime.Now + "]  -  " + status + ": " + logInfo);
            file.Close();
        }

        //used to log the time it took the app to work
        private static void WriteLog(string message, int timeInMilliseconds)
        {
            int timeInSeconds = timeInMilliseconds / 1000;
            int hours = timeInSeconds / 3600;
            int minutes = (timeInMilliseconds % 3600) / 60;
            int seconds = (timeInMilliseconds % 3600) / 3600;

            StreamWriter file = File.AppendText(Resources.TargetDirectory + "\\" + ConfigManager.LogFileName);
            file.WriteLine(message + "{0}:{1}:{2}", hours, minutes, seconds);
            file.Close();
        }
    }
}