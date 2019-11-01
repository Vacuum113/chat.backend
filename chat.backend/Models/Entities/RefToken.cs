using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.backend.Models.Entities
{
    public class RefToken
    { 
        public int Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string ChatUserId { get; set; }
        public ChatUser ChatUser { get; set; }
    }
}
