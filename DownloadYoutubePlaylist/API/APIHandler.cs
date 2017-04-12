using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace DownloadYoutubePlaylist.API
{
    public static class APIHandler
    {
        public static void GetTopTracks()
        {
            WebRequest request = WebRequest.Create(RequestFomatter.GetTopTracksRequest());
            var response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            XMLParser.ParseArtistTopTracks(responseFromServer);

        }

    }
}

