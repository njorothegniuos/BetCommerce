namespace Application.ShoppingCart.Queries.GetShoppingCartById
{
    public sealed record ShoppingCartResponse(Guid Id, string Name, string Image, decimal Price, int Quantity);
}
