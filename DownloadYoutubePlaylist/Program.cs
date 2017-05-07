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

            Thread[] threadArray;
            UIManager.Menu(out threadArray);
            try
            {
                for (int i = 0; i < Resources.Artists.Length; i++)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    ResetGlobalReources();
                    Flags.ManageFlags(Resources.Artists[i]);
                    string artistName = Resources.Artists[i].Split('/')[0].Trim();
                    try
                    {
                        Resources.TrackList = new Stack<string>(APIHandler.GetTopTracks(artistName));
                    }
                    catch
                    {
                        continue;
                    }
                    DirectoryManager.InitTargetDirectory(artistName);

                    for (int j = 0; j < threadArray.Length; j++)
                    {
                        threadArray[j] = new Thread(new ThreadStart(() => ThreadFunction(artistName)));
                        threadArray[j].Start();
                    }

                    foreach (Thread thread in threadArray)
                    {
                        thread.Join();
                    }
                    LogManager.LogFinishWork(sw.Elapsed.Milliseconds);
                }
            }
            catch (Exception ex)
            {
                LogManager.Log(ex.Message, false);
            }
        }

        public static void ThreadFunction(string artistName)
        {
            SeleniumHandler sh = new SeleniumHandler();
            try
            {
                sh.NavigateToConverter();
                sh.DownloadTracks(Resources.TrackList.Pop(), artistName);
                sh.WaitForFilesToDownload();
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

        private static void ResetGlobalReources()
        {
            Resources.Limit = ConfigManager.DefaultLimit;
        }
    }
}