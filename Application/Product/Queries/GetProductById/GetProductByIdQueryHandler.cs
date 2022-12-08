using Application.Abstractions.Messaging;
using Dapper;
using Domain.Exception;
using System.Data;

namespace Application.Product.GetProductById
{
    internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductListingResponse>
    {
        private readonly IDbConnection _dbConnection;

        public GetProductByIdQueryHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<ProductListingResponse> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            const string sql = @"SELECT * FROM ""Products"" WHERE ""Id"" = @productId";

            var webinar = await _dbConnection.QueryFirstOrDefaultAsync<ProductListingResponse>(
                sql,
                new { request.productId });

            if (webinar is null)
            {
                throw new EmailAlertNotFoundException(request.productId);
            }

            return webinar;
        }
    }

}

