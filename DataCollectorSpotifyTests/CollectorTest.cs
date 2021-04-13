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
        public void CollectorConstructor_ValidInputs_ValidObject()
        {
            Collector collector = new Collector();
            Assert.True(null != collector);
        }
    }
}
