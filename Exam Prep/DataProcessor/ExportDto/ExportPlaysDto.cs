using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Play")]
    public class ExportPlaysDto
    {
        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("Duration")]
        
        public string Duration { get; set; }

        [XmlAttribute("Rating")]
        public string Rating { get; set; }

        [XmlAttribute("Genre")]
        public string Genre { get; set; }

        [XmlArray("Actors")]
        public ActorDto[] Actors { get; set; }
    }

   
}
