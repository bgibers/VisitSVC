using System.ComponentModel.DataAnnotations;

namespace Visit.Service.Models.Requests
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Required")] public string Email { get; set; }
    }
}