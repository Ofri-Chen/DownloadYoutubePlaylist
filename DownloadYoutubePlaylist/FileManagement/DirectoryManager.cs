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
