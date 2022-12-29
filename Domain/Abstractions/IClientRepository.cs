using Domain.Entities.ClientModule;

namespace Domain.Abstractions
{
    public interface IClientRepository
    {
        void Insert(Client client);
    }
}
