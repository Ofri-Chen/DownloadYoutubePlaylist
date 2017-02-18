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

namespace DownloadYoutubePlaylist
{
    class Program
    {
        private static IWebDriver _driver;
        private static List<string> _urlList;
        private static List<string> _titles;
        private static int _timeout;
        private static string _baseUrl = "";
        private static string _downloadFolderPath;
        private static string _targetFolderPath;
        private static string _converterUrl = "https://www.onlinevideoconverter.com/video-converter";
        private static int _waitForConversion;
        static void Main(string[] args)
        {
            try
            {
                Initialize();
                Menu();
                FillUrlList();

                for (int i = 0; i < _playlistVideos.Count; i++)
                {
                    ConvertSong(_urlList[i]);
                    if (!DownloadSong())
                    {
                        continue;
                    }
                    SaveTitle();
                }

                ClearDriver(_driver);

                Thread.Sleep(_timeout * 1000);

                MoveFilesToDirectory();


            }
            catch
            {
                ClearDriver(_driver);
            }

        }

        #region Initialize
        private static void Initialize()
        {
            _driver = new ChromeDriver(@"C:\Selenium");
            _timeout = Convert.ToInt32(ConfigurationManager.AppSettings["timeout"]);
            _downloadFolderPath = ConfigurationManager.AppSettings["downloadFolderPath"];
            _targetFolderPath = ConfigurationManager.AppSettings["targetFolderPath"];
            _waitForConversion = Convert.ToInt32(ConfigurationManager.AppSettings["waitForConversion"]);
            InitLists();
        }
        private static void ClearDriver(IWebDriver driver)
        {
            driver.Quit();
        }
        public static void InitLists()
        {
            _playlistVideos = new List<IWebElement>();
            _urlList = new List<string>();
            _titles = new List<string>();
        }
        #endregion
        private static void Menu()
        {
            Console.WriteLine("Enter Playlist url");
            _baseUrl = Console.ReadLine();

            Console.WriteLine("Enter target directory");
            _targetFolderPath += Console.ReadLine();
            Console.WriteLine("Directory's path: " + _targetFolderPath);
        }

        private static void FillUrlList()
        {
            _driver.Navigate().GoToUrl(_baseUrl);
            List <IWebElement> playlistVideos = _driver.FindElements(By.ClassName("playlist-video")).ToList();
            for (int i = 0; i < playlistVideos.Count; i++)
            {
                _urlList.Add(playlistVideos[i].GetAttribute("href"));
            }
        }

        #region Conversion and Download
        private static void FillUrlTextBox(string url)
        {
            IWebElement urlTextBox = _driver.FindElement(By.Id("texturl"));
            urlTextBox.Clear();
            urlTextBox.SendKeys(url);
        }
        private static void ConvertSong(string url)
        {
            _driver.Navigate().GoToUrl(_converterUrl);
            FillUrlTextBox(url);
            _driver.FindElement(By.Id("convert1")).Click();
        }
        private static bool DownloadSong()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(30))
            {
                var windowHandles = _driver.WindowHandles;
                if (windowHandles.Count > 1)
                {
                    //Close pop up tab
                    _driver.SwitchTo().Window(windowHandles[1]);
                    _driver.Close();
                    _driver.SwitchTo().Window(windowHandles[0]);
                }
                try
                {
                    _driver.FindElement(By.Id("downloadq")).Click();
                }
                catch
                {
                    if (sw.Elapsed > TimeSpan.FromSeconds(30))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private static void SaveTitle()
        {
            string title = _driver.FindElement(By.CssSelector(".download-section-1-1-title-content a")).Text;
            MakeTitleViable(ref title);
            _titles.Add(title);
        }
        private static void MakeTitleViable(ref string title)
        {
            // \/:*?"<>|
            string viableTitle = "";
            for (int i = 0; i < title.Length; i++)
            {
                if (title[i] != '\\' ||
                    title[i] != '/' ||
                    title[i] != ':' ||
                    title[i] != '*' ||
                    title[i] != '?' ||
                    title[i] != '"' ||
                    title[i] != '<' ||
                    title[i] != '>' ||
                    title[i] != '|')
                {
                    viableTitle += title[i];
                }
            }
        }
        #endregion

        private static void MoveFilesToDirectory()
        {
            for (int i = 0; i < _titles.Count; i++)
            {
                try
                {
                    File.Move(_downloadFolderPath + "\\" + _titles[i] + ".mp3", _targetFolderPath + "\\" + _titles[i] + ".mp3");
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
