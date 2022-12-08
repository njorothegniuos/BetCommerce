using Domain.Entities.ProductModule;

namespace Domain.Abstractions
{
    public interface IProductRepository
    {
        void Insert(Product product);
    }
}
