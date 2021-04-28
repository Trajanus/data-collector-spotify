using DataCollectorSpotify;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DataCollectorSpotifyTests
{
    public class CollectorTest
    {
        public static string ClientId = "dc60a7c44ec94344af6bfa8b63d99e43";
        [Fact]
        public void Constructor_ValidInputs_ValidObject()
        {
            Collector collector = new Collector(ClientId);
            Assert.True(null != collector);
        }

        [Fact]
        public void GetLastTrackPlayedByUser_ValidUsername_ValidTrack()
        {
            Collector collector = new Collector(ClientId);
            string username = "example-user";
            Track lastTrackPlayed = collector.GetLastTrackPlayedByUser(username);
            Assert.True(null != lastTrackPlayed);
        }
    }
}
