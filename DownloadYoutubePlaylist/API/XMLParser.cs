using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DownloadYoutubePlaylist.API
{
    public static class XMLParser
    {
        public static void ParseArtistTopTracks(string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            var tracks = doc.GetElementsByTagName("name");
            var a = tracks.Cast<XmlNode>();
            var b = a.Select(node => node.InnerXml).Where(trackName => trackName.ToLower() != Resources.ArtistName.ToLower().Replace('_', ' '));
            //List<string> b = new List<string>();

            //foreach (XmlNode item in elemList)
            //{
            //    b.Add(item.InnerXml);
            //}
        }

    }
}
