using FluentValidation;

namespace Application.Token.Commands.CreateToken
{
    public sealed class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
    {
        public CreateTokenCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
