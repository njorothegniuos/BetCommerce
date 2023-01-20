using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SignalR.Web
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _context;
        public ApplicationDbContext(IConfiguration connectionString)
        {
            _context = connectionString;
        }
        public DbSet<ApplicationDbContext> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_context.GetConnectionString("DefaultConnection"));
        }
    }
}
