using System;
using System.Threading;
using DownloadYoutubePlaylist.FileManagement;
using DownloadYoutubePlaylist.API;
using System.Linq;
using System.Collections.Generic;

namespace DownloadYoutubePlaylist
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                UIManager.Menu();

                Resources.TrackList = new Stack<string>(APIHandler.GetTopTracks());

                DirectoryManager.InitTargetDirectory();



                SeleniumHandler.NavigateToConverter();
                SeleniumHandler.DownloadTracks(Resources.TrackList.Pop());

                Thread thread = new Thread(new ThreadStart(ThreadFunction));

            }
            catch (Exception ex)
            {
                LogManager.Log(ex.Message, false);
            }
            finally
            {
                SeleniumHandler.QuitDriver();
            }
        }

        public static void ThreadFunction()
        {

            SeleniumHandler.NavigateToConverter();
            SeleniumHandler.DownloadTracks(Resources.TrackList.Pop());
        }
    }
}