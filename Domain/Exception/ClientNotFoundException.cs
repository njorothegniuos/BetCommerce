using Domain.Exception.Base;

namespace Domain.Exception
{
    public class ClientNotFoundException : NotFoundException
    {
        public ClientNotFoundException(Guid id)
            : base($"The Client with the identifier {id} was not found.")
        {
        }

    }
}
