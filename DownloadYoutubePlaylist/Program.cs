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
            Thread[] threadArray;
            UIManager.Menu(out threadArray);
            try
            {
                for (int i = 0; i < Resources.Artists.Length; i++)
                {
                    //Resources.ArtistName = Resources.Artists[i];
                    Resources.TrackList = new Stack<string>(APIHandler.GetTopTracks(Resources.Artists[i]));
                    DirectoryManager.InitTargetDirectory(Resources.Artists[i]);

                    for (int j = 0; j < threadArray.Length; j++)
                    {
                        threadArray[j] = new Thread(new ThreadStart(() => ThreadFunction(Resources.Artists[i])));
                        threadArray[j].Start();
                    }
                    //Wait for thread to finish - VERY IMPORTANT - otherwise you'll have 564654 chromes open (and you don't want that)
                    foreach (Thread thread in threadArray)
                    {
                        thread.Join();
                    }
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
    }
}