using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace chat.backend.Models.Entities
{
    public class ChatUser : IdentityUser
    {
        public RefToken Token { get; set; }

    }
}
