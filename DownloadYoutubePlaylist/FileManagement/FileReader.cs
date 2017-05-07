using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DownloadYoutubePlaylist.FileManagement
{
    public static class FileReader
    {
        public static string ReadFromFile(string path)
        {
            StreamReader file = new StreamReader(path);
            return file.ReadToEnd();
        }
    }
}