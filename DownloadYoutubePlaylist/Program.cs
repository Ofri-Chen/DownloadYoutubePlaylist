using System;
using System.Threading;
using DownloadYoutubePlaylist.FileManagement;
using DownloadYoutubePlaylist.API;
using System.Collections.Generic;
using System.Diagnostics;

namespace DownloadYoutubePlaylist
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                Thread[] threadArray;
                UIManager.Menu(out threadArray);

                Resources.TrackList = new Stack<string>(APIHandler.GetTopTracks());

                DirectoryManager.InitTargetDirectory();

                for (int i = 0; i < threadArray.Length; i++)
                {
                    threadArray[i] = new Thread(new ThreadStart(ThreadFunction));
                    threadArray[i].Start();
                }
            }
            catch (Exception ex)
            {
                LogManager.Log(ex.Message, false);
            }

            finally
            {
                LogManager.LogFinishWork(sw.Elapsed.Milliseconds);
            }
        }

        public static void ThreadFunction()
        {
            SeleniumHandler sh = new SeleniumHandler();
            try
            {
                sh.NavigateToConverter();
                sh.DownloadTracks(Resources.TrackList.Pop());
                sh.WaitForFilesToDownload();

                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                //while (sw.Elapsed.TotalMilliseconds < ConfigManager.WaitTillDownloadIsFinished * 1000) {
                //    Console.WriteLine("Waiting for {0} seconds to pass", ConfigManager.WaitTillDownloadIsFinished);
                //}
            }
                        
            catch (Exception ex)
            {
                LogManager.Log(ex.Message, false);
            }
            finally
            {
                sh.QuitDriver();
            }

        }
    }
}