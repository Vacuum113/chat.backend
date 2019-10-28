using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.backend.Models.Entities
{
    public class RefToken
    {
        [Key]
        [ForeignKey("ChatUser")]
        public string Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationTime { get; set; }
        public ChatUser ChatUser { get; set; }
    }
}
