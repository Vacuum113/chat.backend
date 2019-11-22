
using System.ComponentModel.DataAnnotations;

namespace chat.backend.ViewModel
{
    public class RefresTokenVievModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
