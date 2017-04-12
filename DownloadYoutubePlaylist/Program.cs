using System;
using System.Threading;
using DownloadYoutubePlaylist.FileManagement;
using DownloadYoutubePlaylist.API;

namespace DownloadYoutubePlaylist
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                UIManager.Menu();

                APIHandler.GetTopTracks();


                DirectoryManager.InitTargetDirectory();
                SeleniumHandler.SetDownloadsDirectoryPath();
                SeleniumHandler.FillUrlList();

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