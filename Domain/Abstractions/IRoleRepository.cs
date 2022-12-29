using Domain.Entities.ClientModule;

namespace Domain.Abstractions
{
    public interface IRoleRepository
    {
        void Insert(Role role);
    }
}
