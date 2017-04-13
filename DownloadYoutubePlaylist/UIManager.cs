using System;
using System.Threading;

namespace DownloadYoutubePlaylist
{
    public static class UIManager
    {
        private const int MAX_THREADS = 4;

        public static void Menu(out Thread [] threadArray)
        {
            //Console.WriteLine("Enter Playlist url");
            //Resources.PlaylistUrl = /*Console.ReadLine();*/"https://www.youtube.com/watch?v=Og5-Pm4HNlI&list=PLAYFVhxsaqDuOh4Ic5mRu5CiZVKCMVv66";

            Console.WriteLine("Enter artist's name");
            Resources.ArtistName = Console.ReadLine();

            //Console.WriteLine("Enter target directory");
            //Resources.TargetDirectory = ConfigManager.BaseTargetDirectoryPath + Console.ReadLine();
            //Console.WriteLine("Downloading to: " + Resources.TargetDirectory);

            Console.WriteLine("How many instances of chrome would you like to work on the job? (Max {0})", MAX_THREADS);
            int numOfThreads;
            try
            {
                numOfThreads = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                numOfThreads = 1;
            }
            if(numOfThreads < 1 || numOfThreads > MAX_THREADS)
            {
                numOfThreads = 1;
            }

            threadArray = new Thread[numOfThreads];
        }
    }
}
