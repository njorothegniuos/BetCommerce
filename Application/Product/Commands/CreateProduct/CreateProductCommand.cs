using Application.Abstractions.Messaging;

namespace Application.Product.Commands.CreateProduct
{
    public sealed record CreateProductCommand(string Name, string Image, decimal Price, int Quantity) : ICommand<Guid>;
}
