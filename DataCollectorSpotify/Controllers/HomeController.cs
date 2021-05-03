using Microsoft.AspNetCore.Mvc;
using PlaylistsNET.Models;
using Serilog;
using SpotifyAPI.Web;
using Swan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;

namespace DataCollectorSpotify.Controllers
{
    [Route("callback")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private Collector _collector;
        private DataCollectorSpotifyOptions _options;
        public HomeController(DataCollectorSpotifyOptions options, Collector collector)
        {
            _options = options;
            _collector = collector;
        }

        [HttpGet]
        public async Task<IActionResult> Callback(string code)
        {
            var initialResponse = await new OAuthClient().RequestToken(
              new PKCETokenRequest(_options.SpotifyDataCollectorClientId
                                    , code
                                    , _options.SpotifyAuthCallbackUri
                                    , _collector.VerifierChallengePair.verifier)
            );

            var spotify = new SpotifyClient(initialResponse.AccessToken);

            var currentUser = await spotify.UserProfile.Current();
            var userPlaylists = await spotify.Playlists.GetUsers(currentUser.Id);

            List<FullPlaylist> userFullPlaylists = new List<FullPlaylist>(userPlaylists.Items.Count);

            foreach (var item in userPlaylists.Items)
            {
                userFullPlaylists.Add(await spotify.Playlists.Get(item.Id));
                await Task.Delay(500);
            }

            List<M3uPlaylist> playlists = new List<M3uPlaylist>(userPlaylists.Items.Count);

            foreach(var userFullPlaylist in userFullPlaylists)
            {
                M3uPlaylist playlist = new M3uPlaylist
                {
                    FileName = userFullPlaylist.Name,
                    IsExtended = true
                };

                foreach (var track in userFullPlaylist.Tracks.Items)
                {
                    FullTrack fullTrack = (FullTrack)track.Track;
                    var playlistEntry = new M3uPlaylistEntry
                    {
                        Album = fullTrack.Album.Name,
                        AlbumArtist = fullTrack.Artists.Select(artist => artist.Name).Aggregate((i, j) => i + ", " + j).TrimEnd().Trim(','),
                        Duration = new TimeSpan(0, 0, 0, 0, fullTrack.DurationMs),
                        Title = fullTrack.Name
                    };

                    playlist.PlaylistEntries.Add(playlistEntry);
                }

                playlists.Add(playlist);
            }

            foreach(var playlist in playlists)
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), playlist.FileName);
                System.IO.File.WriteAllText(filePath
                    , JsonConvert.SerializeObject(playlist));
            }
            
            return StatusCode(200);
        }
    }
}
