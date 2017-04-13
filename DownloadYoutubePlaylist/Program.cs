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
            SeleniumHandler.SetDownloadsDirectoryPath();
            SeleniumHandler.NavigateToConverter();
            SeleniumHandler.SearchVideo("Linkin Park - In The End Lyrics");
            SeleniumHandler.ConvertFirstResult();
            SeleniumHandler.SwitchToResultsWindow();
            SeleniumHandler.ConvertVideo();
            try
            {
                UIManager.Menu();

                Resources.TrackList = new Stack<string>(APIHandler.GetTopTracks());

                DirectoryManager.InitTargetDirectory();
                SeleniumHandler.SetDownloadsDirectoryPath();

                //SeleniumHandler.FillUrlStackByArtist();

                SeleniumHandler.ConvertAndDownload(Resources.UrlStack.Pop());
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
    }
}