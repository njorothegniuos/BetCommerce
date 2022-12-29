using Domain.Common;

namespace Application.Token.Commands.CreateToken
{
    public sealed record CreateTokenRequest(Guid Id, Roles Role);
}
