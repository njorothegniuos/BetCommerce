using Domain.Entities.MessagingModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class EmailAlertConfiguration : IEntityTypeConfiguration<EmailAlert>
    {
        public void Configure(EntityTypeBuilder<EmailAlert> builder)
        {
            builder.ToTable("EmailAlerts");

            builder.HasKey(email => email.Id);

            builder.Property(email => email.To).HasMaxLength(100).IsRequired();
            builder.Property(email => email.Subject).HasMaxLength(100).IsRequired();
            builder.Property(email => email.Body).IsRequired().IsRequired();
        }
    }
}
