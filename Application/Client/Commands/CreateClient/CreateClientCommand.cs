using Application.Abstractions.Messaging;

namespace Application.Client.Commands.CreateClient
{
    public sealed record CreateClientCommand(string Name, string ContactEmail, string Description) : ICommand<CreateClientResponse>;
}
