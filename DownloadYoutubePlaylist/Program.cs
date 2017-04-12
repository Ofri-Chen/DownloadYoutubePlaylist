using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading;
using System.Configuration;
using DownloadYoutubePlaylist.FileManagement;


namespace DownloadYoutubePlaylist
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                UIManager.Menu();
                DirectoryManager.InitTargetDirectory();
                SeleniumHandler.FillUrlList();

                SeleniumHandler.ConvertAndDownload(Resources.UrlStack.Pop());

                Thread.Sleep(ConfigManager.Timeout * 1000);
                MP3Manager.MoveFilesToDirectory();
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