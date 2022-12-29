namespace Application.ShoppingCart.Commands.CreateShoppingCart
{
    public sealed record CreateShoppingCartRequest(Guid UserId, Guid ProductId, int Quantity);
}
