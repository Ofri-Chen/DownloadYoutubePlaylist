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
            DateTime time = DateTime.Now;
            StreamWriter file = File.AppendText(Resources.TargetDirectory + "\\" + ConfigManager.LogFileName);
            //file.WriteLine("[" + DateTime.Now + "]  -  " + status + ": " + logInfo);
            Console.WriteLine("[" + DateTime.Now.ToString("dd/mm/yyyy HH:mm:ss") + "]  -  " + status + ": " + logInfo);
            file.Close();
        }

        //used to log the time it took to download all the artist's songs
        private static void WriteLog(string message, int timeInMilliseconds)
        {
            string logInfo = message + FormatTime(timeInMilliseconds);
            StreamWriter file = File.AppendText(Resources.TargetDirectory + "\\" + ConfigManager.LogFileName);
            file.WriteLine(logInfo);
            file.Close();
        }

        private static string FormatTime(int timeInMilliseconds)
        {
            int timeInSeconds = timeInMilliseconds / 1000;
            string hours = MakeTimeDoubleDigit(timeInSeconds / 3600);
            string minutes = MakeTimeDoubleDigit((timeInMilliseconds % 3600) / 60);
            string seconds = MakeTimeDoubleDigit((timeInMilliseconds % 3600) / 3600);

            return string.Format("{0}:{1}:{2}", hours, minutes, seconds);
        }

        private static string MakeTimeDoubleDigit(int time)
        {
            if(time / 10 == 0)
            {
                return "0" + time;
            }
            return time.ToString();
        }
    }
}