using DataCollectorSpotify;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DataCollectorSpotifyTests
{
    public class CollectorTest
    {
        [Fact]
        public void Constructor_ValidInputs_ValidObject()
        {
            Collector collector = new Collector();
            Assert.True(null != collector);
        }

        [Fact]
        public void GetLastTrackPlayedByUser_ValidUsername_ValidTrack()
        {
            Collector collector = new Collector();
            string username = "example-user";
            Track lastTrackPlayed = collector.GetLastTrackPlayedByUser(username);
            Assert.True(null != lastTrackPlayed);
        }
    }
}
