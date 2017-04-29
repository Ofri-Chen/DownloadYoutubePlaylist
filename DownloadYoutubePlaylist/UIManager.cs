using System;
using System.Threading;
using System.Collections.Generic;

namespace DownloadYoutubePlaylist
{
    public static class UIManager
    {
        private const int MAX_THREADS = 4;

        public static void Menu(out Thread [] threadArray)
        {
            ReadArtistAndLimit();
            threadArray = new Thread[ReadNumOfThreads()];
        }

        private static void ReadArtistAndLimit()
        {
            Console.WriteLine("Enter artist's name");
            string line = Console.ReadLine();

            if (Flags.AreThereFlags(line))
            {

                string[] flags = line.Split('/');
                for (int i = 1; i < flags.Length; i++)
                {
                    Flags.FlagManager(flags[i]);
                }

                Resources.ArtistName = flags[0].Trim();
            }
            else
            {
                Resources.ArtistName = line.Trim();
            }
        }

        private static int ReadNumOfThreads()
        {
            Console.WriteLine("How many instances of chrome would you like to work on the job? (Max {0})", MAX_THREADS);
            int numOfThreads;
            try
            {
                numOfThreads = Convert.ToInt32(Console.ReadLine());
                if (numOfThreads < 1 || numOfThreads > MAX_THREADS)
                {
                    numOfThreads = 1;
                }
            }
            catch
            {
                numOfThreads = 1;
            }


            return numOfThreads;
        }
    }
}