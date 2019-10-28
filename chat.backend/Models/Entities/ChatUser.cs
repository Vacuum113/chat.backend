using Microsoft.AspNetCore.Identity;

namespace chat.backend.Models.Entities
{
    public class ChatUser : IdentityUser
    {
        public RefToken Token { get; set; }
    }
}
