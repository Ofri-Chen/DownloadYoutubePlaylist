using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadYoutubePlaylist.API
{
    public static class RequestFomatter
    {
        public static string GetTopTracksRequest()
        {
            return ConfigManager.APIBaseUrl + ConfigManager.APIGetTopTracks.Replace("Artist_Name", Resources.ArtistName)
                .Replace("API_KEY", ConfigManager.APIAuthorizationKey);
        }
    }
}
