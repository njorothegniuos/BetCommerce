using Domain.Exception.Base;

namespace Domain.Exception
{
    public class TokenNotFoundException : NotFoundException
    {
        public TokenNotFoundException(Guid id)
            : base($"The token with the identifier {id} was not found.")
        {
        }
    }
}
