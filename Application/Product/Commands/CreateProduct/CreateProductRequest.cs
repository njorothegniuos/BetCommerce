namespace Application.Product.Commands.CreateProduct
{
    public sealed record CreateProductRequest(string Name, string Image, decimal Price, int Quantity);
}
