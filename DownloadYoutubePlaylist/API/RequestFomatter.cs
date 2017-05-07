using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadYoutubePlaylist.API
{
    public static class RequestFomatter
    {
        public static string GetTopTracksRequest(string artistName)
        {
            return ConfigManager.APIBaseUrl + ConfigManager.APIGetTopTracks.Replace("Artist_Name", artistName)
                .Replace("API_KEY", ConfigManager.APIAuthorizationKey);
        }
    }
}