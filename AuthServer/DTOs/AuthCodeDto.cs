

using System.Text.Json.Serialization;

namespace AuthServer.DTOs
{
    public class AuthCodeDto
    {

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("code_challenge")]
        public string CodeChallenge { get; set; }

        [JsonPropertyName("code_challenge_method")]
        public string CodeChallengeMethod { get; set; }

        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }
        public DateTime Expiry { get; set; }
    }
}