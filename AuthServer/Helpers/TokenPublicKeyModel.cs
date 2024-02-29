
using AuthServer.Entities;

namespace AuthServer.Helpers
{
    public class TokenPublicKeyModel
    {
        public string AccessToken { get; set; }
        public UserKey PublicKey { get; set; }
    }
}