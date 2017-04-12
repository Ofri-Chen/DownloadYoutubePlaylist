using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DownloadYoutubePlaylist.FileManagement
{
    public static class DirectoryManager
    {
        public static void InitTargetDirectory()
        {
            Directory.CreateDirectory(Resources.TargetDirectory);
        }
    }
}
