using DataCollectorSpotify;
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
            Track track = new Track(title, album, artist);

            Assert.True(null != track);

            Assert.True(!string.IsNullOrWhiteSpace(track.Title));
            Assert.True(track.Title == title);

            Assert.True(!string.IsNullOrWhiteSpace(track.Album));
            Assert.True(track.Album == album);

            Assert.True(!string.IsNullOrWhiteSpace(track.Artist));
            Assert.True(track.Artist == artist);
        }
    }
}
