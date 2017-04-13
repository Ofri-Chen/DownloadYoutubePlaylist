using System.Collections.Generic;

namespace DownloadYoutubePlaylist
{
    public static class Resources
    {
        public static string ArtistName { get; set; }

        public static string PlaylistUrl { get; set; }

        public static string TargetDirectory { get; set; }

        //public static string ConverterUrl = "https://www.onlinevideoconverter.com/video-converter";

        public static string ConverterUrl = "http://convert2mp3.net/";

        public static Stack<string> TrackList;

        public static Stack<string> UrlStack = new Stack<string>();

        public static List<string> TitleList = new List<string>();
    }
}
