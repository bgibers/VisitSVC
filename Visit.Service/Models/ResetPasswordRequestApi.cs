using System.ComponentModel.DataAnnotations;

namespace Visit.Service.Models
{
    public class ResetPasswordRequestApi
    {
        [Required(ErrorMessage = "Required")] public string Email { get; set; }
    }
}