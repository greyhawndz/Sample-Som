using System;
using System.Xml;
using System.Collections.Generic;

namespace Sample_Som
{
    class Writer
    {
        public static void write(List<Song> songs)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            using (XmlWriter writer = XmlWriter.Create("Mapping.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("SongCollection");
                foreach (Song song in songs)
                {
                    writer.WriteStartElement("Song");
                    writer.WriteElementString("SongId", song.SongID.ToString());
                    writer.WriteElementString("Title", song.Title);
                    writer.WriteElementString("Artist", song.Artist);
                    writer.WriteElementString("Album", song.Album);
                    writer.WriteElementString("TrackNumber", song.TrackNumber.ToString());
                    writer.WriteElementString("Year", song.Year);
                    writer.WriteStartElement("GenreCollection");
                    writer.WriteElementString("Genre", song.Genre);
                    writer.WriteEndElement();
                    writer.WriteElementString("Path", song.Path);
                    writer.WriteElementString("FeaturePath", song.FeaturePath);
                    writer.WriteElementString("x", song.XPos.ToString());
                    writer.WriteElementString("y", song.YPos.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
