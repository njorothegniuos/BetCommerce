using Application.Abstractions.Messaging;
using Domain.Common;

namespace Application.Token.Commands.CreateToken
{
    public sealed record CreateTokenCommand(Guid Id, Roles Role) : ICommand<TokenResponse>;
}
