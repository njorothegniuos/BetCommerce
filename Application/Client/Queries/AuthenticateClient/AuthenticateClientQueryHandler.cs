using Application.Abstractions.Messaging;
using Dapper;
using Domain.Exception;
using System.Data;

namespace Application.Client.Queries.AuthenticateClient
{
    internal class AuthenticateClientQueryHandler : IQueryHandler<AuthenticateClientQuery, AuthenticatedClientResponse>
    {
        private readonly IDbConnection _dbConnection;

        public AuthenticateClientQueryHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<AuthenticatedClientResponse> Handle(
            AuthenticateClientQuery request,
            CancellationToken cancellationToken)
        {
            const string sql = @"SELECT Id,Role FROM ""Clients"" WHERE ""Id"" = @apiKey";

            var client = await _dbConnection.QueryFirstOrDefaultAsync<AuthenticatedClientResponse>(
                sql,
                new { request.apiKey });

            if (client is null)
            {
                throw new ClientNotFoundException(request.apiKey);
            }

            return client;
        }
    }
}
