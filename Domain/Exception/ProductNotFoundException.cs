using Domain.Exception.Base;

namespace Domain.Exception
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid id)
            : base($"The Product with the identifier {id} was not found.")
        {
        }
    }
}
