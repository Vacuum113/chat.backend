
using System.ComponentModel.DataAnnotations;

namespace chat.backend.Models.ViewModel
{
    public class RefresTokenVievModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
