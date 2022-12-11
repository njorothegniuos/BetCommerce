using Application.Abstractions.Messaging;

namespace Application.Role.Commands.CreateRole
{
    public sealed record CreateRoleCommand(Guid Id, DateTime CreatedAt, DateTime ModifiedAt) : ICommand<Guid>;
}
