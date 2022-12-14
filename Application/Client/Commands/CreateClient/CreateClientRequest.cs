using Domain.Common;

namespace Application.Client.Commands.CreateClient
{
    public sealed record CreateClientRequest(string Name, string ContactEmail, string Description);
}
