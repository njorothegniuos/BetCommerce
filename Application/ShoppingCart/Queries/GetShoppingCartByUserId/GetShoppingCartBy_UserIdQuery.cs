using Application.Abstractions.Messaging;

namespace Application.ShoppingCart.Queries.GetShoppingCartById
{
    public sealed record GetShoppingCartBy_UserIdQuery(Guid userId) : IQuery<List<ShoppingCartResponse>>;
}
