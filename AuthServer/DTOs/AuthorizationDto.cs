using Microsoft.AspNetCore.Mvc;

namespace AuthServer.DTOs
{
    public class AuthorizationDto
    {
        [FromQuery(Name = "response_type")]
        public string ResponseType { get; set; }

        [FromQuery(Name = "client_id")]
        public string ClientId { get; set; }

        [FromQuery(Name = "code_challenge")]
        public string CodeChallenge { get; set; }

        [FromQuery(Name = "code_challenge_method")]
        public string CodeChallengeMethod { get; set; }

        [FromQuery(Name = "redirect_uri")]
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
    }
}