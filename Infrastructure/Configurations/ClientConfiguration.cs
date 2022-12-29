using Domain.Entities.ClientModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Secret).HasMaxLength(128);
            builder.Property(x => x.Description).HasMaxLength(250);
            builder.Property(x => x.ContactEmail).HasMaxLength(150);
        }
    }
}
