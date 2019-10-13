using System.ComponentModel.DataAnnotations;

namespace chat.backend.Models.ViewModel
{
    public class CreateChatUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
