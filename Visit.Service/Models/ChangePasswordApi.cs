using System.ComponentModel.DataAnnotations;

namespace Visit.Service.Models
{
    public class ChangePasswordApi
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")] public string OldPassword { get; set; }

        [Required(ErrorMessage = "Required")] public string NewPassword { get; set; }
    }
}