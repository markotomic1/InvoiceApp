

using System.Text.Json.Serialization;

namespace AuthServer.DTOs
{
    public class OpenIdConfigurationDto
    {

        public string Issuer { get; set; }

        [JsonPropertyName("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; }

        public string JwksUri { get; set; }
    }
}