using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadYoutubePlaylist
{
    static class Flags
    {
        public static bool AreThereFlags(string command)
        {
            return command.Contains('/');
        }

        public static void FlagManager(string command)
        {
            switch (command[0])
            {
                case 'l':
                    Limit(command.Substring(2, command.Length - 2));
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        public static void Limit(string limitStr)
        {
            int limit;
            try
            {
                limit = Convert.ToInt32(limitStr);
                if(limit >= 50 || limit <= 1)
                {
                    limit = 50;
                }
            }
            catch
            {
                limit = 50;
            }

            Resources.Limit = limit;
        }
    }
}