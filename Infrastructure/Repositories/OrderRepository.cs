using Domain.Abstractions;
using Domain.Entities.OrderModule;

namespace Infrastructure.Repositories
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public void Insert(Order order) => _dbContext.Set<Order>().Add(order);
    }
}
