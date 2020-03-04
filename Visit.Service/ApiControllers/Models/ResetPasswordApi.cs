using System.ComponentModel.DataAnnotations;

namespace Visit.Service.ApiControllers.Models
{
    public class ResetPasswordApi
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        
        [Required(ErrorMessage = "Required")]
        public string NewPassword { get; set; }
    }
}