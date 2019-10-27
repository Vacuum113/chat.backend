using System.ComponentModel.DataAnnotations;

namespace chat.backend.Models.ViewModel
{
    public class EditEmailChatUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string OldEmail { get; set; }
        [Required]
        public string NewEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
