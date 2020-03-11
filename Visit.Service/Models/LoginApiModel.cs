using Visit.Service.Models.Validators;
using FluentValidation.Attributes;

namespace Visit.Service.Models
{
    [Validator(typeof(LoginValidator))]
    public class LoginApiModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}