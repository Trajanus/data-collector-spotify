using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DataCollectorSpotifyTests
{
    public class ProgramTest
    {
        [Fact]
        public void Program_ValidArgs_ExitsSuccessfully()
        {
            string[] args = new string[0];
            int exitCode = DataCollectorSpotify.Program.Main(args);
            Assert.True(0 == exitCode);
        }

        //[Fact]
        //public void Program_ValidArgs_ExitsSuccessfully()

    }
}
