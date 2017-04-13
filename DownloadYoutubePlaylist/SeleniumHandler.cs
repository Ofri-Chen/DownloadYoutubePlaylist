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

        private const int WAIT_FOR_CONVERTION = 20;

        #region public methods
        //public static void ConvertAndDownload(string url)
        //{
        //    _driver.Navigate().GoToUrl(Resources.ConverterUrl);
        //    FillUrlTextBox(url);
        //    bool convertionFlag = true;

        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    while (sw.Elapsed < TimeSpan.FromSeconds(20))
        //    {
        //        ClosePopUpTabs();
        //        try
        //        {
        //            _driver.FindElement(By.Id("convert1")).Click();
        //            break;
        //        }
        //        catch
        //        {
        //            if (sw.Elapsed > TimeSpan.FromSeconds(20))
        //            {
        //                LogManager.Log("Took too long for the convert button to appear", false);
        //                convertionFlag = false;
        //            }
        //        }
        //    }

        //    if (convertionFlag)
        //    {
        //        if (DownloadSong())
        //        {
        //            string title = GetTitle();
        //            LogManager.Log(title, true);
        //            SaveTitle(title);
        //        }

        //        try
        //        {
        //            ConvertAndDownload(Resources.UrlStack.Pop());
        //        }
        //        catch (Exception ex)
        //        {
        //            LogManager.Log(ex.Message, false);
        //        }
        //    }
        //}

        public static void NavigateToConverter()
        {
            _driver.Navigate().GoToUrl(Resources.ConverterUrl);
        }

        public static void DownloadTracks(string trackName)
        {
            try
            {
                SearchVideo(trackName + " Lyrics");
                ClosePopUpTabs();
                ClickOnResult();
                SwitchToResultsWindow();
                ConvertVideo();
                SetTrackName(trackName);
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

            DownloadTracks(Resources.TrackList.Pop());
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

            _driver = new ChromeDriver(service, options);
        }

        public static void QuitDriver()
        {
            _driver.Quit();
        }
        #endregion

        #region private methods

        private static void ConvertAndDownloadTrack(string trackName)
        {

        }

        private static void SearchVideo(string title)
        {
            _driver.FindElement(By.Id("search")).SendKeys(title);
            _driver.FindElement(By.Name("searchForm")).FindElement(By.ClassName("mainbtn")).Click();
        }

        private static string GetVideoTitle(int index = 0)
        {
            return _driver.FindElements(By.CssSelector(".searchtitle b"))[index].Text;
        }

        private static void ClickOnResult(int index = 0)
        {
            _driver.FindElements(By.CssSelector(".row.content .row .span7 .mainbtna"))[index].Click();
        }

        private static void ConvertVideo()
        {
            _driver.FindElement(By.CssSelector("#convertForm [type = submit]")).Click();
        }

        private static void SetTrackName(string trackName)
        {
            _driver.FindElement(By.CssSelector("#input_artist .btn")).Click();
            var input = _driver.FindElement(By.Id("inputArtist"));
            input.Clear();
            input.SendKeys(Resources.ArtistName);

            _driver.FindElement(By.CssSelector("#input_title .btn")).Click();
            input = _driver.FindElement(By.Id("inputTitle"));
            input.Clear();
            input.SendKeys(trackName);

            _driver.FindElement(By.CssSelector(".controls .btn-success")).Click();
        }

        private static void Download()
        {
            _driver.FindElement(By.CssSelector(".span7 .btn-success")).Click();

        }

        private static void SwitchToResultsWindow()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles[1]);
        }

        private static void GoToConverterMainPage()
        {
            _driver.FindElement(By.ClassName("brand")).Click();
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
        #endregion

        //public static void FillUrlStack(string playlistUrl)
        //{
        //    _driver.Navigate().GoToUrl(playlistUrl);
        //    List<IWebElement> playlistVideos = _driver.FindElements(By.ClassName("playlist-video")).ToList();
        //    for (int i = 0; i < playlistVideos.Count; i++)
        //    {
        //        Resources.UrlStack.Push(playlistVideos[i].GetAttribute("href"));
        //    }
        //}

        //public static void FillUrlStackByArtist(int numOfTracks = 50)
        //{
        //    _driver.Navigate().GoToUrl("https://www.youtube.com/");
        //    var youtubeSearchField = _driver.FindElement(By.Id("masthead-search-term"));
        //    youtubeSearchField.Clear();


        //    youtubeSearchField.SendKeys(Resources.ArtistName + " - " + Resources.);
        //}


        //private static void FillUrlTextBox(string url)
        //{
        //    IWebElement urlTextBox = _driver.FindElement(By.Id("texturl"));
        //    urlTextBox.Clear();
        //    urlTextBox.SendKeys(url);
        //}
        //private static bool DownloadSong()
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    while (sw.Elapsed < TimeSpan.FromSeconds(30))
        //    {
        //        ClosePopUpTabs();
        //        try
        //        {
        //            _driver.FindElement(By.Id("downloadq")).Click();
        //            break;
        //        }
        //        catch
        //        {
        //            if (sw.Elapsed > TimeSpan.FromSeconds(30))
        //            {
        //                LogManager.Log("Took too long to convert", false);
        //                return false;
        //            }
        //        }
        //    }

        //    return true;
        //}
        //private static void SaveTitle(string title)
        //{
        //    MakeTitleViable(ref title);
        //    Resources.TitleList.Add(title);
        //}

        //private static string GetTitle()
        //{
        //    return _driver.FindElement(By.CssSelector(".download-section-1-1-title-content a")).Text;
        //}
        //private static void MakeTitleViable(ref string title)
        //{
        //    // \/:*?"<>|
        //    string viableTitle = "";
        //    for (int i = 0; i < title.Length; i++)
        //    {
        //        if (title[i] != '\\' ||
        //            title[i] != '/' ||
        //            title[i] != ':' ||
        //            title[i] != '*' ||
        //            title[i] != '?' ||
        //            title[i] != '"' ||
        //            title[i] != '<' ||
        //            title[i] != '>' ||
        //            title[i] != '|')
        //        {
        //            viableTitle += title[i];
        //        }
        //    }
        //}



    }
}
