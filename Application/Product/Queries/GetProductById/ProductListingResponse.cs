namespace Application.Product.GetProductById
{
    public sealed record ProductListingResponse(Guid Id, string Name, string Image, decimal Price, int Quantity,DateTime CreateDate);
}
