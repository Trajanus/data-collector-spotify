using PlaylistLibrary;
using System;
using Xunit;

namespace DataCollectorSpotifyTests
{
    public class TrackTest
    {
        [Fact]
        public void Constructor_ValidInputs_ValidObject()
        {
            string title = "test-title";
            string album = "test-album";
            string artist = "test-artist";
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, 100000);
            Track track = new Track(title, album, artist, duration);

            Assert.True(null != track);

            Assert.True(!string.IsNullOrWhiteSpace(track.Title));
            Assert.True(track.Title == title);

            Assert.True(!string.IsNullOrWhiteSpace(track.Album));
            Assert.True(track.Album == album);

            Assert.True(!string.IsNullOrWhiteSpace(track.Artist));
            Assert.True(track.Artist == artist);

            Assert.True(null != track.Duration);
            Assert.True(track.Duration.TotalMilliseconds == duration.TotalMilliseconds);
        }
    }
}
