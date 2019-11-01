
using System.ComponentModel.DataAnnotations;

namespace chat.backend.Models.ViewModel
{
    public class RefresTokenVievModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
