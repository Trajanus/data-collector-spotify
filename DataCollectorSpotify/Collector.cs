using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollectorSpotify
{
    public class Collector
    {
        private string _clientId;
        private (string verifier, string challenge) _verifierChallengePair;
        public (string verifier, string challenge) VerifierChallengePair { get { return _verifierChallengePair; } }

        public Collector(string clientId)
        {
            _clientId = clientId;
            // Generates a secure random verifier of length 120 and its challenge
            _verifierChallengePair = PKCEUtil.GenerateCodes(120);
        }

        public Uri GetLoginRequestUri(Uri callbackUri)
        {
            // Make sure callbackUri is in your applications redirect URIs in the SpotifyAPI UI
            var loginRequest = new LoginRequest(
              callbackUri,
              _clientId,
              LoginRequest.ResponseType.Code
            )
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = _verifierChallengePair.challenge,
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
            };
            return loginRequest.ToUri();
        }
    }
}
