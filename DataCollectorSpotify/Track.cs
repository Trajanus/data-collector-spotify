using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollectorSpotify
{
    public class Track
    {
        public string Album { get; }
        public string Artist { get; }
        public string Title { get; }

        public Track(string title, string album, string artist)
        {
            Album = album;
            Title = title;
            Artist = artist;
        }
    }
}
