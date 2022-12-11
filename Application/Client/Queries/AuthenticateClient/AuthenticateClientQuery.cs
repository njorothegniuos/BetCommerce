using Application.Abstractions.Messaging;

namespace Application.Client.Queries.AuthenticateClient
{
    public sealed record AuthenticateClientQuery(Guid apiKey) : IQuery<AuthenticatedClientResponse>;
}
