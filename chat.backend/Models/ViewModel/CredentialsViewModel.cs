using System.ComponentModel.DataAnnotations;

namespace chat.backend.Controllers
{
    public class CredentialsViewModel
    {
        [Required(ErrorMessage = "No email specified.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}