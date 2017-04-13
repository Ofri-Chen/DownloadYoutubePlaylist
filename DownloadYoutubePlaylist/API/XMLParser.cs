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
        public static IEnumerable<string> ParseArtistTopTracks(string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            return doc.GetElementsByTagName("name")
                .Cast<XmlNode>()
                .Select(node => node.InnerXml)
                .Where(trackName => trackName.ToLower() != Resources.ArtistName.ToLower()
                .Replace('_', ' '));
        }
    }
}
