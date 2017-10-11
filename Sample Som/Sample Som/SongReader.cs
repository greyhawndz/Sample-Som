using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace Sample_Som
{
    class SongReader
    {
        public List<Song> songs { get; set; }

        //private XmlTextReader xmlReader;
        //private XmlNodeType xmlNode;
        private Song song;
        public SongReader()
        {
            songs = new List<Song>();
        }

        public void ReadXML(String filename)
        {
            List<Song> temp = new List<Song>();
            temp = (from e in XDocument.Load(filename).Element("SongCollection").Elements("Song")
                     select new Song
                     {
                         SongID = (int)e.Element("SongId"),
                         Title = (string)e.Element("Title"),
                         Artist = (string)e.Element("Artist"),
                         Album = (string)e.Element("Album"),
                         TrackNumber = (int)e.Element("TrackNumber"),
                         Year = (string)e.Element("Year"),
                         Genre =  (string)e.Element("GenreCollection").Descendants().First(),
                         Path = (string)e.Element("Path"),
                         FeaturePath = (string) e.Element("FeaturePath")
                     }).ToList();
            foreach(Song song in temp)
            {
                songs.Add(song);
            }
            foreach(Song song in songs)
            {
                song.ExtractFeatures();
            }

        }
    }
}
