using System;
using System.Collections.Generic;
using System.Text;

namespace PlaylistLibrary
{
    public class Playlist
    {
        public string Name { get; }
        public IList<Track> Tracks { get; }
        public DateTime CreateDate { get; }


    }
}
