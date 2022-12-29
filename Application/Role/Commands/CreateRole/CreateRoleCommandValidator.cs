using FluentValidation;

namespace Application.Role.Commands.CreateRole
{
    public sealed  class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {

        }
    }
}
