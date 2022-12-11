using Domain.Common;
using Domain.Entities.ClientModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configurations
{
    public class TokenConfiguration : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.ToTable("Tokens");

            builder.Property(x => x.SubjectId).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Role).HasConversion(new EnumToStringConverter<Roles>());
        }
    }
}
