using Domain.Entities.OrderModule;

namespace Domain.Abstractions
{
    public interface IOrderRepository
    {
        void Insert(Order product);
    }
}
