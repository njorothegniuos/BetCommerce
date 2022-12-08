namespace Application.Product.GetProductById
{
    public sealed record ProductListingResponse(Guid Id, string Name, string Image, string Price, string Quantity);
}
