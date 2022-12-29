using Domain.Abstractions;
using Domain.Entities.ClientModule;

namespace Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TokenRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public void Insert(Token token) => _dbContext.Set<Token>().Add(token);
    }
}
