using Microsoft.AspNetCore.Mvc;
using Serilog;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;

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
            var track = await spotify.Tracks.Get("1s6ux0lNiTziSrd7iUAADH");
            Log.Information(track.Name);
            return StatusCode(200);
        }
    }
}
