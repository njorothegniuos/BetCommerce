using Domain.Entities.ShoppingCartModule;

namespace Domain.Abstractions
{
    public interface IShoppingCartRepository
    {
        void Insert(ShoppingCart shoppingCart);
        void Delete(ShoppingCart shoppingCart);
    }
}
