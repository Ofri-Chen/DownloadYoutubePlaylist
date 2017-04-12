using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadYoutubePlaylist
{
    public static class Resources
    {
        public static string PlaylistUrl { get; set; }

        public static string TargetDirectory { get; set; }

        public static string ConverterUrl = "https://www.onlinevideoconverter.com/video-converter";

        public static Stack<string> UrlStack = new Stack<string>();

        public static List<string> TitleList = new List<string>();
    }
}
