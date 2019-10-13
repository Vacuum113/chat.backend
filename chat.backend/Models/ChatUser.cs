using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.backend.Models
{
    public class ChatUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Int64 ChatUserID { get; set; }
    }
}
