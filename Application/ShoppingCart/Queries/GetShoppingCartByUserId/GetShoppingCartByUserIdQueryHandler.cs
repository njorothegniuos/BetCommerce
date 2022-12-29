using Application.Abstractions.Messaging;
using Dapper;
using Domain.Exception;
using System.Data;

namespace Application.ShoppingCart.Queries.GetShoppingCartById
{
    internal class GetShoppingCartBy_UserIdQueryHandler : IQueryHandler<GetShoppingCartBy_UserIdQuery, List<ShoppingCartResponse>>
    {
        private readonly IDbConnection _dbConnection;

        public GetShoppingCartBy_UserIdQueryHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<List<ShoppingCartResponse>> Handle(
            GetShoppingCartBy_UserIdQuery request,
            CancellationToken cancellationToken)
        {
            string sql = $"SELECT C.[Id],       P.NAME,	   P.IMAGE     ,P.PRICE      ,C.[Quantity]  FROM [BetCommerceMainStore].[dbo].[ShoppingCarts] C INNER JOIN [dbo].[Products] P ON C.[ProductId] = P.Id WHERE C.UserId = '{request.userId}'";

            var shoppingCartResponse = await _dbConnection.QueryAsync<ShoppingCartResponse>(
                sql,
                null);

            if (shoppingCartResponse is null)
            {
                throw new ShoppingCartNotFoundException(request.userId);
            }

            return shoppingCartResponse.ToList();
        }
    }
}
