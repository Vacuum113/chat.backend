using Microsoft.AspNetCore.Identity;
using System;

namespace chat.backend.Data.IdentityUserAsp
{
    public class IdentUser : IdentityUser
    {
        public string RefreshToken { get; set; }
        
        public DateTime? ExpirationTime { get; set; }
    }
}
