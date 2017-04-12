using System;

namespace DownloadYoutubePlaylist
{
    public static class UIManager
    {
        public static void Menu()
        {
            //Console.WriteLine("Enter Playlist url");
            //Resources.PlaylistUrl = /*Console.ReadLine();*/"https://www.youtube.com/watch?v=Og5-Pm4HNlI&list=PLAYFVhxsaqDuOh4Ic5mRu5CiZVKCMVv66";

            Console.WriteLine("Enter artist's name");
            Resources.ArtistName = Console.ReadLine();

            Console.WriteLine("Enter target directory");
            Resources.TargetDirectory = ConfigManager.BaseTargetDirectoryPath + Console.ReadLine();
            Console.WriteLine("Directory's path: " + Resources.TargetDirectory);
        }
    }
}
