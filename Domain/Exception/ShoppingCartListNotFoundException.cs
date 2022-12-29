using Domain.Exception.Base;

namespace Domain.Exception
{
    public class ShoppingCartListNotFoundException : NotFoundException
    {
        public ShoppingCartListNotFoundException()
            : base($"The Shopping Cart item(s)  was not found.")
        {
        }

    }
}
