
using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class DiscoveryDocument
    {
        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [JsonPropertyName("jwksUri")]
        public string JwksUri { get; set; }
    }
}