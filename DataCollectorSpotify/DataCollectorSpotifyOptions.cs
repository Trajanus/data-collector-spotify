using Serilog.Events;
using System;
using System.IO;

namespace DataCollectorSpotify
{
    public class DataCollectorSpotifyOptions
    {
        public LogEventLevel SerilogLogEventLevel { get; set; }
        public string LogDirectoryPath { get; set; }
        public Uri SpotifyAuthCallbackUri { get; set; }
        public string SpotifyDataCollectorClientId { get; set; }
        public string BrowserPath { get; set; }
    }
}
