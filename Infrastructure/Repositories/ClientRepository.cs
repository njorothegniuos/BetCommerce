using Domain.Abstractions;
using Domain.Entities.ClientModule;

namespace Infrastructure.Repositories
{
    public class ClientRepository: IClientRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ClientRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public void Insert(Client client) => _dbContext.Set<Client>().Add(client);
    }
}
