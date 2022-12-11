using Domain.Abstractions;
using Domain.Entities.ClientModule;

namespace Infrastructure.Repositories
{
    public  class RoleRepository: IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public void Insert(Role role) => _dbContext.Set<Role>().Add(role);
    }
}
