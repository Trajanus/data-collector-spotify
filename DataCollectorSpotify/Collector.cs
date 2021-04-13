using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollectorSpotify
{
    public class Collector
    {
        public Track GetLastTrackPlayedByUser(string username)
        {
            string trackTitle = GetLastTrackPlayedByUserTitle(username);
            string trackAlbum = GetLastTrackPlayedByUserAlbum(username);
            string trackArtist = GetLastTrackPlayedByUserAlbum(username);
            return new Track(trackTitle, trackAlbum, trackArtist);
        }

        private string GetLastTrackPlayedByUserTitle(string username)
        {
            return "the-last-track";
        }

        private string GetLastTrackPlayedByUserAlbum(string username)
        {
            return "the-last-album";
        }

        private string GetLastTrackPlayedByUserArtist(string username)
        {
            return "the-last-artist";
        }
    }
}
