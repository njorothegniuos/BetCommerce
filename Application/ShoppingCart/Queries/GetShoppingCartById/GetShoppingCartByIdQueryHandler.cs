using Application.Abstractions.Messaging;
using Application.ShoppingCart.Commands.CreateShoppingCart;
using Dapper;
using Domain.Entities.ProductModule;
using Domain.Entities.ShoppingCartModule;
using Domain.Exception;
using Mapster;
using System.Data;

namespace Application.ShoppingCart.Queries.GetShoppingCartById
{
    internal class GetShoppingCartByIdQueryHandler : IQueryHandler<GetShoppingCartByIdQuery, ShoppingCartResponse>
    {
        private readonly IDbConnection _dbConnection;

        public GetShoppingCartByIdQueryHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<ShoppingCartResponse> Handle(
            GetShoppingCartByIdQuery request,
            CancellationToken cancellationToken)
        {
            string sql = $"SELECT C.[Id],       P.NAME,	   P.IMAGE     ,P.PRICE      ,C.[Quantity]  FROM [BetCommerceMainStore].[dbo].[ShoppingCarts] C INNER JOIN [dbo].[Products] P ON C.[ProductId] = P.Id WHERE C.Id = '{request.shoppingCartId}'";
            
            var shoppingCartResponse = await _dbConnection.QueryFirstOrDefaultAsync<ShoppingCartResponse>(
            sql,
            new { request.shoppingCartId });

            if (shoppingCartResponse is null)
            {
                throw new ShoppingCartNotFoundException(request.shoppingCartId);
            }

            var command = shoppingCartResponse.Adapt<ShoppingCartResponse>();

            return shoppingCartResponse;
        }
    }
}
