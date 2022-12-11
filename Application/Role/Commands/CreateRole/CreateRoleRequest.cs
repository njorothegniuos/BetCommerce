using Application.Abstractions.Messaging;

namespace Application.Role.Commands.CreateRole
{
    public sealed record CreateRoleRequest(Guid Id, DateTime CreatedAt, DateTime ModifiedAt) : ICommand<Guid>;
}
