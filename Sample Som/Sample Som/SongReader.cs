using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Sample_Som
{
    class SongReader
    {
        public List<Song> songs { get; set; }

        private XmlReader xmlReader;
        private String xmlNode;
        public SongReader()
        {
            songs = new List<Song>();
            xmlReader = XmlReader.Create(new StringReader(xmlNode));

        }
    }
}
