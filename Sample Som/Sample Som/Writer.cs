using System;
using System.Xml;

class Writer
{
    class Song
    {
        int _songId = 0;
        string _title = "";
        string _artist = "";
        string _album = "";
        int _trackNumber = 0;
        string _year = "";
        string _genre = "";
        string _path = "";
        string _featurePath = "";
        int _x = 0;
        int _y = 0;

        public Song(int id, string title, string artist, string album, int tracknumber, string year, string genre, string path, string featurepath, int x, int y)
        {
            this._songId = id;
            this._title = title;
            this._artist = artist;
            this._album = album;
            this._trackNumber = tracknumber;
            this._year = year;
            this._genre = genre;
            this._path = path;
            this._featurePath = featurepath;
            this._x = x;
            this._y = y;
        }
        public int Id { get { return _songId; } }
        public string Title { get { return _title; } }
        public string Artist { get { return _artist; } }
        public string Album { get { return _album; } }
        public int TrackNumber { get { return _trackNumber; } }
        public string Year { get { return _year; } }
        public string Genre { get { return _genre; } }
        public string Path { get { return _path; } }
        public string FeaturePath { get { return _featurePath; } }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
    }
    /* USED FOR TESTING
      static void Main(string[] args)
    {
        Console.WriteLine("Going to write");
        Song[] songs = new Song[1];
        Song song = new Song(1, "Despacito", "Fonzi", "Worst Album", 69, "2017", "Shit", @"\Documents", @"\Ye", 8, 2);
        songs[0] = song;
        write(songs);
    }
    */
    static void write(Song[] songs)
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.NewLineOnAttributes = true;
        using (XmlWriter writer = XmlWriter.Create("test.xml", settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("SongCollection");
            foreach (Song song in songs)
            {
                writer.WriteStartElement("Song");
                writer.WriteElementString("SongId", song.Id.ToString());
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
                writer.WriteElementString("x", song.X.ToString());
                writer.WriteElementString("y", song.Y.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }
}