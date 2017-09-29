using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace Sample_Som
{
    class SongReader
    {
        public List<Song> songs { get; set; }

        private XmlTextReader xmlReader;
        private XmlNodeType xmlNode;
        private Song song;
        public SongReader()
        {
            songs = new List<Song>();
        }

        public void ReadXML(String filename)
        {
            xmlReader = new XmlTextReader(filename);
            while (xmlReader.Read())
            {
                xmlNode = xmlReader.NodeType;

                if (xmlNode == XmlNodeType.Element)
                {
                    song = new Song();
                    if (xmlReader.Name == "SongId")
                    {
                        xmlReader.Read();
                        song.SongID = int.Parse(xmlReader.Value);
                        Debug.WriteLine("id " + song.SongID);
                    }
                    if (xmlReader.Name == "Title")
                    {
                        xmlReader.Read();
                        song.Title = xmlReader.Value;
                        Debug.WriteLine("Title: " + song.Title);
                    }
                    if (xmlReader.Name == "Artist")
                    {
                        xmlReader.Read();
                        song.Artist = xmlReader.Value;
                        Debug.WriteLine("Artist: " + song.Artist);
                    }
                    if(xmlReader.Name == "Album")
                    {
                        xmlReader.Read();
                        song.Album = xmlReader.Value;
                        Debug.WriteLine("Album: " + song.Album);
                    }
                    if(xmlReader.Name == "TrackNumber")
                    {
                        xmlReader.Read();
                        song.TrackNumber = int.Parse(xmlReader.Value);
                        Debug.WriteLine("Track Number: " + song.TrackNumber);
                    }
                    if(xmlReader.Name == "Year")
                    {
                        xmlReader.Read();
                        song.Year = xmlReader.Value;
                        Debug.WriteLine("Year: " + song.Year);
                    }
                    if(xmlReader.Name == "GenreCollection")
                    {
                        xmlReader.Read();
                        Debug.WriteLine("Skipped genre collection");
                    }
                    if(xmlReader.Name == "Genre")
                    {
                        xmlReader.Read();
                        song.Genre = xmlReader.Value;
                        Debug.WriteLine("Genre: " + song.Genre);
                        xmlReader.Read();
                    }
                    if(xmlReader.Name == "Path")
                    {
                        xmlReader.Read();
                        song.Path = xmlReader.Value;
                        Debug.WriteLine("Path: " + song.Path);
                    }
                    if(xmlReader.Name == "FeaturePath")
                    {
                        xmlReader.Read();
                        song.FeaturePath = xmlReader.Value;
                        Debug.WriteLine("Feature Path: " + song.FeaturePath);
                    }
                    if(song.FeaturePath != null)
                    {
                        songs.Add(song);
                    }
                }
            }
            Debug.WriteLine("Song Count: " + songs.Count);
        }
    }
}
