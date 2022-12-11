using Domain.Common;

namespace Application.Client.Queries.AuthenticateClient
{
    public sealed record AuthenticatedClientResponse(Guid Id, Roles role);
}
