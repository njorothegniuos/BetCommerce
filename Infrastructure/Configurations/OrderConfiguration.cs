using Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(order => order.Id);

            builder.Property(order => order.OderNumber).HasMaxLength(150).IsRequired();
            builder.Property(order => order.OderTotals).HasColumnType("DECIMAL(18, 2)").IsRequired();
        }
    }
}
