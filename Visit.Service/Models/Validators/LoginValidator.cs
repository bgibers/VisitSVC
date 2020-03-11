using FluentValidation;

namespace Visit.Service.Models.Validators
{
    public class LoginValidator : AbstractValidator<LoginApiModel>
    {
        public LoginValidator() 
        {
            RuleFor(vm => vm.UserName).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.Password).Length(6, 12).WithMessage("Password must be between 6 and 12 characters");
        }
    }
}