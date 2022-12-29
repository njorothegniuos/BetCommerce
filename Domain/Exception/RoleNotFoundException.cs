using Domain.Exception.Base;

namespace Domain.Exception
{
    public class RoleNotFoundException : NotFoundException
    {
        public RoleNotFoundException(Guid id)
            : base($"The role with the identifier {id} was not found.")
        {
        }

    }
}
