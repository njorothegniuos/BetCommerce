using Application.Abstractions.Messaging;
using Dapper;
using Domain.Exception;
using System.Data;

namespace Application.Client.Queries.GetClientById
{
    internal class GetClientByIdQueryHandler : IQueryHandler<GetClientByIdQuery, ClientResponse>
    {
        private readonly IDbConnection _dbConnection;

        public GetClientByIdQueryHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<ClientResponse> Handle(
            GetClientByIdQuery request,
            CancellationToken cancellationToken)
        {
            const string sql = @"SELECT * FROM ""Clients"" WHERE ""Id"" = @clientId";

            var productListingResponse = await _dbConnection.QueryFirstOrDefaultAsync<ClientResponse>(
                sql,
                new { request.clientId });

            if (productListingResponse is null)
            {
                throw new ClientNotFoundException(request.clientId);
            }

            return productListingResponse;
        }
    }
}
