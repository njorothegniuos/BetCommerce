using Application.Abstractions.Messaging;
using Application.Product.GetProductById;
using Dapper;
using Domain.Exception;
using System.Data;

namespace Application.Product.Queries.GetProducts
{
    internal class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, List<ProductListingResponse>>
    {
        private readonly IDbConnection _dbConnection;

        public GetProductsQueryHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<List<ProductListingResponse>> Handle(
            GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            const string sql = @"SELECT * FROM Products";

            var shoppingCartResponse = await _dbConnection.QueryAsync<ProductListingResponse>(
                sql,null);

            if (shoppingCartResponse is null)
            {
                throw new ShoppingCartListNotFoundException();
            }

            return shoppingCartResponse.ToList();
        }
    }
}
