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

namespace DownloadYoutubePlaylist
{
    class Program
    {
        private static IWebDriver _driver;
        private static List<IWebElement> _playlistVideos;
        private static List<string> _urlList;
        private static List<string> _titles;
        private static string _timeout;
        private static string _baseUrl = "";
        private static string _downloadFolderPath = @"C:\Users\ofric\Downloads";
        private static string _targetFolderPath = @"C:\Users\ofric\Desktop\Music\";
        private static string _converterUrl = "https://www.onlinevideoconverter.com/video-converter";
        static void Main(string[] args)
        {
            Menu();
            InitLists();
            FillVideosList();
            FillUrlList();

            for (int i = 0; i < _playlistVideos.Count; i++)
            {
                _driver.Navigate().GoToUrl(_converterUrl);
                FillUrlTextBox(_urlList[i]);
                _driver.FindElement(By.Id("convert1")).Click();

                if (!DownloadSong())
                {
                    continue;
                }

                string title = _driver.FindElement(By.CssSelector(".download-section-1-1-title-content a")).Text;
                MakeTitleViable(ref title);

                _titles.Add(title);
            }

            ClearDriver(_driver);

            Thread.Sleep(Convert.ToInt32(_timeout)*1000);

            for (int i = 0; i < _titles.Count; i++)
            {
                try
                {
                    File.Move(_downloadFolderPath + "\\" + _titles[i] + ".mp3", _targetFolderPath + "\\" + _titles[i] + ".mp3");
                }
                catch
                {
                }
            }
        }

        public static void MakeTitleViable(ref string title)
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

        public static void Menu()
        {
            Console.WriteLine("Enter Playlist url");
            _baseUrl = Console.ReadLine();

            Console.WriteLine("Enter target directory");
            _targetFolderPath += Console.ReadLine();
            Console.WriteLine("Directory's path: " + _targetFolderPath);

            Console.WriteLine("Enter Timeout");
            _timeout = Console.ReadLine();
            if (_timeout.ToString() == "")
            {
                _timeout = "30";
            }
        }

        public static void InitLists()
        {
            _playlistVideos = new List<IWebElement>();
            _urlList = new List<string>();
            _titles = new List<string>();
        }

        public static void FillVideosList()
        {
            _driver = new ChromeDriver(@"C:\Selenium");
            _driver.Navigate().GoToUrl(_baseUrl);
            _playlistVideos = _driver.FindElements(By.ClassName("playlist-video")).ToList();
        }

        public static void FillUrlList()
        {
            for (int i = 0; i < _playlistVideos.Count; i++)
            {
                _urlList.Add(_playlistVideos[i].GetAttribute("href"));
            }
        }

        public static void FillUrlTextBox(string url)
        {
            IWebElement urlTextBox = _driver.FindElement(By.Id("texturl"));
            urlTextBox.Clear();
            urlTextBox.SendKeys(url);
        }

        public static bool DownloadSong()
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
        public static void ClearDriver(IWebDriver driver)
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
