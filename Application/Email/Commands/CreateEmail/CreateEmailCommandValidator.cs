using FluentValidation;

namespace Application.Email.Commands.CreateEmail
{
    public sealed class CreateEmailCommandValidator : AbstractValidator<CreateEmailCommand>
    {
        public CreateEmailCommandValidator()
        {
            RuleFor(x => x.To).NotEmpty();

            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
