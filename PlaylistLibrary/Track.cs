using System;
using System.Linq;
using PlaylistsNET;
using PlaylistsNET.Models;
using SpotifyAPI.Web;

namespace PlaylistLibrary
{
    public class Track
    {
        public string Album { get; }
        public string Artist { get; }
        public string Title { get; }
        public TimeSpan Duration { get; }

        public Track(string title, string album, string artist, TimeSpan duration)
        {
            Album = album;
            Title = title;
            Artist = artist;
            Duration = duration;
        }

        public Track(M3uPlaylistEntry playlistEntry)
        {
            Album = playlistEntry.Album;
            Title = playlistEntry.Title;
            Artist = playlistEntry.AlbumArtist;
            Duration = playlistEntry.Duration;
        }

        public Track(FullTrack fullTrack)
        {
            Album = fullTrack.Album.Name;
            Artist = fullTrack.Artists.Select(artist => artist.Name).Aggregate((i, j) => i + ", " + j).TrimEnd().Trim(',');
            Duration = new TimeSpan(0, 0, 0, 0, fullTrack.DurationMs);
            Title = fullTrack.Name;
        }

        public static M3uPlaylistEntry SpotifyTrackToM3uPlaylistEntry(FullTrack fullTrack)
        {
            if(null == fullTrack)
            {
                throw new ArgumentNullException($"Provided ${nameof(FullTrack)} argument is null.");
            }
            if(null == fullTrack.Album)
            {
                throw new ArgumentNullException($"Provided ${nameof(FullTrack.Album)} argument is null.");
            }
            if(null == fullTrack.Artists)
            {
                throw new ArgumentNullException($"Provided ${nameof(FullTrack.Artists)} argument is null.");
            }

            return new M3uPlaylistEntry
            {
                Album = fullTrack.Album.Name,
                AlbumArtist = fullTrack.Artists.Select(artist => artist.Name).Aggregate((i, j) => i + ", " + j).TrimEnd().Trim(','),
                Duration = new TimeSpan(0, 0, 0, 0, fullTrack.DurationMs),
                Title = fullTrack.Name
            };
        }
    }
}
