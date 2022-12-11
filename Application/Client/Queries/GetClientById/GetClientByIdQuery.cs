using Application.Abstractions.Messaging;

namespace Application.Client.Queries.GetClientById
{
    public sealed record GetClientByIdQuery(Guid clientId) : IQuery<ClientResponse>;
}
