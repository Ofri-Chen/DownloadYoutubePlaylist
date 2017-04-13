using System.IO;

namespace DownloadYoutubePlaylist.FileManagement
{
    public static class DirectoryManager
    {
        public static void InitTargetDirectory()
        {
            Resources.TargetDirectory = ConfigManager.BaseTargetDirectoryPath + Resources.ArtistName;
            Directory.CreateDirectory(Resources.TargetDirectory);
        }
    }
}
