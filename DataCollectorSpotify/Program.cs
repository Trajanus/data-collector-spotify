using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PlaylistLibrary;
using PlaylistsNET.Models;
using Serilog;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollectorSpotify
{
    public class Program
    {
        public static DataCollectorSpotifyOptions options;
        private static readonly ILogger log = Log.ForContext<Program>();

        public static int Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            Collector collector = host.Services.GetService<Collector>();
            options = host.Services.GetService<DataCollectorSpotifyOptions>();
            host.Start();

            Uri loginRequestUri = collector.GetLoginRequestUri(options.SpotifyAuthCallbackUri);

            Process.Start(options.BrowserPath, loginRequestUri + " --new-window");

            return 0;
        }

        public static async void AuthenticatedHandler(object sender, AuthenticationEventArgs e)
        {
            var spotify = new SpotifyClient(e.AccessToken);
            var playlists = await GetUserPlaylists(spotify);

            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(DataCollectorSpotify));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            foreach (var playlist in playlists)
            {
                string playlistFileName = Path.GetInvalidFileNameChars().Aggregate(playlist.FileName, (current, c) => current.Replace(c.ToString(), string.Empty)); 
                string filePath = Path.Combine(directoryPath, $"{playlistFileName}.json");
                File.WriteAllText(filePath, JsonConvert.SerializeObject(playlist));
                log.Information($"Wrote playlist {playlistFileName} to {filePath}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task<List<M3uPlaylist>> GetUserPlaylists(SpotifyClient spotify)
        {
            List<M3uPlaylist> allUserPlaylists = new List<M3uPlaylist>();

            var currentUser = await spotify.UserProfile.Current();
            var userPlaylistPage = await spotify.Playlists.GetUsers(currentUser.Id);

            Stopwatch timeSinceLastRequest = new Stopwatch();

            while(!string.IsNullOrEmpty(userPlaylistPage.Next))
            {
                timeSinceLastRequest.Start();
                allUserPlaylists.AddRange(await GetUserPlaylistPage(userPlaylistPage, spotify));

                if (timeSinceLastRequest.ElapsedMilliseconds < options.MillisecondDelayBetweenRequests)
                {
                    await Task.Delay((int)(options.MillisecondDelayBetweenRequests - timeSinceLastRequest.ElapsedMilliseconds));
                }
                timeSinceLastRequest.Restart();

                userPlaylistPage = await spotify.NextPage(userPlaylistPage);

                if(allUserPlaylists.Count == userPlaylistPage.Total)
                {
                    break;
                }
            }

            log.Information($"Retrieved {allUserPlaylists.Count} user playlists.");
            return allUserPlaylists;
        }

        private static async Task<List<M3uPlaylist>> GetUserPlaylistPage(Paging<SimplePlaylist> userPlaylists, SpotifyClient spotify)
        {
            List<FullPlaylist> userFullPlaylists = new List<FullPlaylist>(userPlaylists.Items.Count);

            foreach (var item in userPlaylists.Items)
            {
                userFullPlaylists.Add(await spotify.Playlists.Get(item.Id));
                await Task.Delay(options.MillisecondDelayBetweenRequests);
            }

            List<M3uPlaylist> playlists = new List<M3uPlaylist>(userPlaylists.Items.Count);

            foreach (var userFullPlaylist in userFullPlaylists)
            {
                M3uPlaylist playlist = new M3uPlaylist
                {
                    FileName = userFullPlaylist.Name,
                    IsExtended = true
                };

                foreach (var track in userFullPlaylist.Tracks.Items)
                {
                    FullTrack fullTrack = (FullTrack)track.Track;
                    var playlistEntry = Track.SpotifyTrackToM3uPlaylistEntry(fullTrack);

                    playlist.PlaylistEntries.Add(playlistEntry);
                }

                playlists.Add(playlist);
            }

            return playlists;
        }
    }
}
