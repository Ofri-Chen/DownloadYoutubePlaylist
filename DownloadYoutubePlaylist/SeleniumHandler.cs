using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using DownloadYoutubePlaylist.FileManagement;
using System.Threading;

namespace DownloadYoutubePlaylist
{
    public class SeleniumHandler
    {
        private IWebDriver _driver;
        private const int WAIT_FOR_CONVERTION = 20;
        private Object lockObj = new Object();

        #region public methods

        public SeleniumHandler()
        {
            SetDownloadsDirectoryPath();
            NavigateToConverter();
        }

        public void NavigateToConverter()
        {
            _driver.Navigate().GoToUrl(Resources.ConverterUrl);
        }

        public void DownloadTracks(string trackName, string artistName)
        {
            try
            {
                SearchVideo(trackName + GetLyricsKeyword(artistName));
                ClosePopUpTabs();
                ClickOnResult();
                SwitchToResultsWindow();
                ConvertVideo();
                SetTrackName(trackName, artistName);
                Download();
                LogManager.Log(trackName, true);
            }
            catch (Exception ex)
            {
                LogManager.Log(ex.Message, false);
            }

            _driver.Close();
            _driver.SwitchTo().Window(_driver.WindowHandles[0]);
            GoToConverterMainPage();

            string song;
            lock (lockObj)
            {
                if (Resources.TrackList.Count > 0)
                {
                    song = Resources.TrackList.Pop();
                }
                else
                {
                    return;
                }
            }
            DownloadTracks(song, artistName);
        }

        public void WaitForFilesToDownload()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed > TimeSpan.FromSeconds(ConfigManager.WaitTillDownloadIsFinished) ||
                DirectoryManager.CheckIfThereAreUnfinishedDownloads()) ;
        }

        public void QuitDriver()
        {
            _driver.Quit();
        }
        #endregion

        #region private methods

        private void SetDownloadsDirectoryPath()
        {
            var service = ChromeDriverService.CreateDefaultService(ConfigManager.ChromeDriverPath);

            var downloadPrefs = new Dictionary<string, object>
                {
                    {"default_directory", Resources.TargetDirectory},
                    {"directory_upgrade", true}
                };

            var options = new ChromeOptions();
            options.AddUserProfilePreference("download", downloadPrefs);

            _driver = new ChromeDriver(service, options);
        }

        private void SearchVideo(string title)
        {
            _driver.FindElement(By.Id("search")).SendKeys(title);
            _driver.FindElement(By.Name("searchForm")).FindElement(By.ClassName("mainbtn")).Click();
        }

        private string GetVideoTitle(int index = 0)
        {
            return _driver.FindElements(By.CssSelector(".searchtitle b"))[index].Text;
        }

        private void ClickOnResult(int index = 0)
        {
            _driver.FindElements(By.CssSelector(".row.content .row .span7 .mainbtna"))[index].Click();
        }

        private void ConvertVideo()
        {
            _driver.FindElement(By.CssSelector("#convertForm [type = submit]")).Click();
        }

        private void SetTrackName(string trackName, string artistName)
        {
            _driver.FindElement(By.CssSelector("#input_artist .btn")).Click();
            var input = _driver.FindElement(By.Id("inputArtist"));
            input.Clear();
            input.SendKeys(artistName);

            _driver.FindElement(By.CssSelector("#input_title .btn")).Click();
            input = _driver.FindElement(By.Id("inputTitle"));
            input.Clear();
            input.SendKeys(trackName);

            _driver.FindElement(By.CssSelector(".controls .btn-success")).Click();
        }

        private void Download()
        {
            _driver.FindElement(By.CssSelector(".span7 .btn-success")).Click();

        }

        private void SwitchToResultsWindow()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles[1]);
        }

        private void GoToConverterMainPage()
        {
            _driver.FindElement(By.ClassName("brand")).Click();
        }

        private void ClosePopUpTabs()
        {
            var windowHandles = _driver.WindowHandles;
            for (int i = 1; i < windowHandles.Count; i++)
            {
                _driver.SwitchTo().Window(windowHandles[1]);
                _driver.Close();
            }
            _driver.SwitchTo().Window(windowHandles[0]);
        }

        private string GetLyricsKeyword(string artistName)
        {
            if(artistName[0] > 'א' && artistName[0] < 'ת')
            {
                return "";
            }
            else
            {
                return "Lyrics";
            }
        }
        #endregion
    }
}