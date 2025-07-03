using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.TransactionExternalId).IsUnique();
            builder.Property(t => t.Status).HasConversion<string>();
        }
    }
}
