using Application.Abstractions.Messaging;

namespace Application.ShoppingCart.Queries.GetShoppingCartById
{
    public sealed record GetShoppingCartByIdQuery(Guid shoppingCartId) : IQuery<ShoppingCartResponse>;
}
