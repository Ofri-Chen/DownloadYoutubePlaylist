using System.IO;

namespace DownloadYoutubePlaylist.FileManagement
{
    public static class DirectoryManager
    {
        public static void InitTargetDirectory(string artistName)
        {
            Resources.TargetDirectory = ConfigManager.BaseTargetDirectoryPath + artistName;
            Directory.CreateDirectory(Resources.TargetDirectory);
        }

        public static bool CheckIfThereAreUnfinishedDownloads()
        {
            return Directory.GetFiles(Resources.TargetDirectory, "*.crdownload").Length > 0; 
        }
    }
}