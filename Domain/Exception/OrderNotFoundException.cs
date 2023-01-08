using Domain.Exception.Base;

namespace Domain.Exception
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid id)
            : base($"The Order with the identifier {id} was not found.")
        {
        }
    }
}
