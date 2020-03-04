using System.ComponentModel.DataAnnotations;

namespace Visit.Service.ApiControllers.Models
{
    public class ResetPasswordRequestApi
    {
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

    }
}