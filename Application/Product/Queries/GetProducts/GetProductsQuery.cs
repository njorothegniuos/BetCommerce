using Application.Abstractions.Messaging;
using Application.Product.GetProductById;

namespace Application.Product.Queries.GetProducts
{
    public class GetProductsQuery : IQuery<List<ProductListingResponse>>
    {
    }
}
