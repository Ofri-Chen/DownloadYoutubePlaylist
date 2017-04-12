using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DownloadYoutubePlaylist.FileManagement
{
    public static class MP3Manager
    {
        public static void MoveFilesToDirectory()
        {
            for (int i = 0; i < Resources.TitleList.Count; i++)
            {
                try
                {
                    string fileName = Resources.TitleList[i] + ".mp3";
                    File.Move(ConfigManager.DownloadFolderPath + "\\" + fileName, Resources.TargetDirectory + "\\" + fileName);
                }
                catch (Exception ex)
                {
                    LogManager.Log(ex.Message, false);
                    continue;
                }
            }
        }
    }
}
