using Domain.Exception.Base;

namespace Domain.Exception
{
    public class ShoppingCartNotFoundException : NotFoundException
    {
        public ShoppingCartNotFoundException(Guid id)
            : base($"The ShoppingCart with the identifier {id} was not found.")
        {
        }

    }
}
