using Application.Abstractions.Messaging;

namespace Application.Product.GetProductById
{
    public sealed record GetProductByIdQuery(Guid productId) : IQuery<ProductListingResponse>;
}
