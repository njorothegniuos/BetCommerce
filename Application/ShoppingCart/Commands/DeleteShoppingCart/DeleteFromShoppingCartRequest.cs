namespace Application.ShoppingCart.Commands.DeleteShoppingCart
{
    public sealed record DeleteFromShoppingCartRequest(Guid Id, Guid UserId, Guid ProductId, int Quantity);
}
