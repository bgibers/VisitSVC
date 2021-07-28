using FluentValidation.Attributes;
using Visit.Service.Models.Validators;

namespace Visit.Service.Models.Requests
{
    [Validator(typeof(LoginValidator))]
    public class LoginApiRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}