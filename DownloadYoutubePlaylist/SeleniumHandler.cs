using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using DownloadYoutubePlaylist.FileManagement;

namespace DownloadYoutubePlaylist
{
    public static class SeleniumHandler
    {
        private static IWebDriver _driver/* = new ChromeDriver(ConfigManager.ChromeDriverPath)*/;

        public static void ConvertAndDownload(string url)
        {
            _driver.Navigate().GoToUrl(Resources.ConverterUrl);
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
                    if (sw.Elapsed > TimeSpan.FromSeconds(20))
                    {
                        LogManager.Log("Took too long for the convert button to appear", false);
                        convertionFlag = false;
                    }
                }
            }

            if (convertionFlag)
            {
                if (DownloadSong())
                {
                    string title = GetTitle();
                    LogManager.Log(title, true);
                    SaveTitle(title);
                }

                try
                {
                    ConvertAndDownload(Resources.UrlStack.Pop());
                }
                catch (Exception ex)
                {
                    LogManager.Log(ex.Message, false);
                }
            }
        }

        public static void FillUrlList()
        {
            _driver.Navigate().GoToUrl(Resources.PlaylistUrl);
            List<IWebElement> playlistVideos = _driver.FindElements(By.ClassName("playlist-video")).ToList();
            for (int i = 0; i < playlistVideos.Count; i++)
            {
                Resources.UrlStack.Push(playlistVideos[i].GetAttribute("href"));
            }
        }

        public static void SetDownloadsDirectoryPath()
        {
            var service = ChromeDriverService.CreateDefaultService(ConfigManager.ChromeDriverPath);

            var downloadPrefs = new Dictionary<string, object>
                {
                    {"default_directory", Resources.TargetDirectory},
                    {"directory_upgrade", true}
                };

            var options = new ChromeOptions();
            options.AddUserProfilePreference("download", downloadPrefs);

            _driver =  new ChromeDriver(service, options);
        }

        public static void QuitDriver()
        {
            _driver.Quit();
        }

        private static void FillUrlTextBox(string url)
        {
            IWebElement urlTextBox = _driver.FindElement(By.Id("texturl"));
            urlTextBox.Clear();
            urlTextBox.SendKeys(url);
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
                        LogManager.Log("Took too long to convert", false);
                        return false;
                    }
                }
            }

            return true;
        }
        private static void SaveTitle(string title)
        {
            MakeTitleViable(ref title);
            Resources.TitleList.Add(title);
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



    }
}
