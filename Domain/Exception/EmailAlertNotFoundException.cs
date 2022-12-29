using Domain.Exception.Base;

namespace Domain.Exception
{
    public class EmailAlertNotFoundException : NotFoundException
    {
        public EmailAlertNotFoundException(Guid id)
            : base($"The EmailAlert with the identifier {id} was not found.")
        {
        }
    }
}
