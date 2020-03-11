using System.ComponentModel.DataAnnotations;

namespace Visit.Service.Models
{
    public class CodeConfirmApi
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")] public string Code { get; set; }
    }
}