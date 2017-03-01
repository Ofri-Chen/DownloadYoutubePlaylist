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
        private static Stack<string> _urls;
        private static List<string> _titles;
        private static int _timeout;
        private static string _baseUrl;
        private static string _downloadFolderPath;
        private static string _targetFolderPath;
        private static string _converterUrl = "https://www.onlinevideoconverter.com/video-converter";
        public static string _logFileName;
        private static int _waitForConversion;
        static void Main(string[] args)
        {
            try
            {
                ReadConfig();
                Menu();
                Initialize();
                FillUrlList();

                ConvertAndDownload(_urls.Pop());

                Thread.Sleep(_timeout * 1000);
                MoveFilesToDirectory();
            }
            catch(Exception ex)
            {
                LogException(ex.Message);
            }
            finally
            {
                _driver.Quit();
            }
        }

        #region Initialize
        private static void Initialize()
        {
            InitTargetDirectory();
            InitDriver();
            InitLists();
        }
        public static void ReadConfig()
        {
            _downloadFolderPath = ConfigurationManager.AppSettings["downloadFolderPath"];
            _targetFolderPath = ConfigurationManager.AppSettings["targetFolderPath"];
            _logFileName = ConfigurationManager.AppSettings["logFileName"];
            _timeout = Convert.ToInt32(ConfigurationManager.AppSettings["timeout"]);
            _waitForConversion = Convert.ToInt32(ConfigurationManager.AppSettings["waitForConversion"]);
        }

        public static void InitDriver()
        {
            _driver = new ChromeDriver(@"C:\Selenium");
        }
        public static void InitLists()
        {
            _urls = new Stack<string>();
            _titles = new List<string>();
        }

        public static void InitTargetDirectory()
        {
            Directory.CreateDirectory(_targetFolderPath);
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
            List<IWebElement> playlistVideos = _driver.FindElements(By.ClassName("playlist-video")).ToList();
            for (int i = 0; i < playlistVideos.Count; i++)
            {
                _urls.Push(playlistVideos[i].GetAttribute("href"));
            }
        }

        #region Conversion and Download
        private static void FillUrlTextBox(string url)
        {
            IWebElement urlTextBox = _driver.FindElement(By.Id("texturl"));
            urlTextBox.Clear();
            urlTextBox.SendKeys(url);
        }
        private static void ConvertAndDownload(string url)
        {
            _driver.Navigate().GoToUrl(_converterUrl);
            FillUrlTextBox(url);
            bool convertionFlag = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(20))
            {
                ClosePopUpTabs();
                try
                {
                    _driver.FindElement(By.Id("convert1")).Click();
                    break;
                }
                catch
                {
                    if (sw.Elapsed > TimeSpan.FromSeconds(20)) ;
                    {
                        LogException("Took too long for the convert button to appear");
                        convertionFlag = false;
                    }
                }
            }

            if (convertionFlag)
            {
                if (DownloadSong())
                {
                    string title = GetTitle();
                    LogSucess(title);
                    SaveTitle(title);
                }

                try
                {
                    ConvertAndDownload(_urls.Pop());
                }
                catch (Exception ex)
                {
                    LogException(ex.Message);
                }
            }
        }
        private static bool DownloadSong()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(30))
            {
                ClosePopUpTabs();
                try
                {
                    _driver.FindElement(By.Id("downloadq")).Click();
                    break;
                }
                catch
                {
                    if (sw.Elapsed > TimeSpan.FromSeconds(30))
                    {
                        LogException("Took too long to convert");
                        return false;
                    }
                }
            }

            return true;
        }
        private static void SaveTitle(string title)
        {
            MakeTitleViable(ref title);
            _titles.Add(title);
        }

        private static string GetTitle()
        {
            return _driver.FindElement(By.CssSelector(".download-section-1-1-title-content a")).Text;
        }
        private static void ClosePopUpTabs()
        {
            var windowHandles = _driver.WindowHandles;
            for (int i = 1; i < windowHandles.Count; i++)
            {
                _driver.SwitchTo().Window(windowHandles[1]);
                _driver.Close();
            }
            _driver.SwitchTo().Window(windowHandles[0]);
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
                catch(Exception ex)
                {
                    LogException(ex.Message);
                    continue;
                }
            }
        }

        #region Log
        private static void LogSucess(string title)
        {
            Log("Sucess", title);
        }

        private static void LogException(string message)
        {
            Log("Exception", message);
        }
        private static void Log(string status, string logInfo)
        {
            StreamWriter file = File.AppendText(_targetFolderPath + "\\" + _logFileName);
            file.WriteLine("[" + DateTime.Now + "]  -  " + status + ": " + logInfo);
            file.Close();
        }
        #endregion
    }
}