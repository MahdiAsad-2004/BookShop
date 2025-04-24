
using FluentValidation;

namespace BookShop.Application.Features.User.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(a => a.Username)
                .NotEmpty();
            
            RuleFor(a => a.Password)
                .NotEmpty();

        }
    }
}
