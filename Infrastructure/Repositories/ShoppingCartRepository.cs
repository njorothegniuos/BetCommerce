using Domain.Abstractions;
using Domain.Entities.ShoppingCartModule;

namespace Infrastructure.Repositories
{
    public sealed class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingCartRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public void Insert(ShoppingCart shoppingCart) => _dbContext.Set<ShoppingCart>().Add(shoppingCart);
    }
}
