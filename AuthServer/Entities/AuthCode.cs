

using System.ComponentModel.DataAnnotations;

namespace AuthServer.Entities
{
    public class AuthCode
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; }
        public string CodeChallenge { get; set; }
        public string CodeChallengeMethod { get; set; }
        public string RedirectUri { get; set; }
        public bool Used { get; set; } = false;
        public DateTime Expiry { get; set; }
    }
}