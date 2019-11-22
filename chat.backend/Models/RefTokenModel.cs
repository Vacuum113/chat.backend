using System;

namespace chat.backend.Models
{
    public class RefToken
    { 
        public string RefreshToken { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
