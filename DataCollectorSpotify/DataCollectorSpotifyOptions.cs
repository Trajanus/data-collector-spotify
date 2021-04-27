using Serilog.Events;

namespace DataCollectorSpotify
{
    public class DataCollectorSpotifyOptions
    {
        public LogEventLevel SerilogLogEventLevel { get; set; }
        public string LogDirectoryPath { get; set; }
    }
}
