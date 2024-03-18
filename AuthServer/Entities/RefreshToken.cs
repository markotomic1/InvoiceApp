using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string RefreshTokenString { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsValid { get; set; }
    }
}