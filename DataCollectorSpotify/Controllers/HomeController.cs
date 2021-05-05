using Microsoft.AspNetCore.Mvc;
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

        public event EventHandler<AuthenticationEventArgs> Authenticated = Program.AuthenticatedHandler;

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

            EventHandler<AuthenticationEventArgs> handler = Authenticated;
            handler?.Invoke(this, new AuthenticationEventArgs { AccessToken = initialResponse.AccessToken });

            return StatusCode(200);
        }
    }
}
