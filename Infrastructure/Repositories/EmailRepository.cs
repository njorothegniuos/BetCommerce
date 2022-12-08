using Domain.Abstractions;
using Domain.Entities.MessagingModule;

namespace Infrastructure.Repositories
{
    public sealed class EmailRepository : IEmailRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmailRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public void Insert(EmailAlert email) => _dbContext.Set<EmailAlert>().Add(email);
    }
}
