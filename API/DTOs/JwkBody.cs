
using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class JwkBody
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("keyId")]
        public string KeyId { get; set; }

        [JsonPropertyName("publicKey")]
        public string PublicKey { get; set; }
    }
}