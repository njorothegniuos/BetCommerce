using FluentValidation;

namespace Application.Client.Commands.CreateClient
{
    public sealed class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.ContactEmail).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
