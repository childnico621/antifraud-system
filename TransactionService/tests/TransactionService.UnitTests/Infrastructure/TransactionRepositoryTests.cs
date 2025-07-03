using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Persistence;
using Xunit;

namespace TransactionService.UnitTests.Infrastructure
{
    public class TransactionRepositoryTests
    {
        private static DbContextOptions<TransactionDbContext> CreateInMemoryOptions()
        {
            return new DbContextOptionsBuilder<TransactionDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateAsync_ShouldAddTransaction()
        {
            var options = CreateInMemoryOptions();
            var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 1, 1000m);

            using (var context = new TransactionDbContext(options))
            {
                var repo = new TransactionRepository(context);
                await repo.CreateAsync(transaction);
            }

            using (var context = new TransactionDbContext(options))
            {
                var saved = await context.Transactions.FirstOrDefaultAsync();
                Assert.NotNull(saved);
                Assert.Equal(transaction.TransactionExternalId, saved!.TransactionExternalId);
            }
        }

        [Fact]
        public async Task GetByExternalIdAsync_ShouldReturnTransaction_WhenExists()
        {
            var options = CreateInMemoryOptions();
            var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 2, 800m);

            using (var context = new TransactionDbContext(options))
            {
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();
            }

            using (var context = new TransactionDbContext(options))
            {
                var repo = new TransactionRepository(context);
                var result = await repo.GetByExternalIdAsync(transaction.TransactionExternalId);

                Assert.NotNull(result);
                Assert.Equal(transaction.TransactionExternalId, result!.TransactionExternalId);
            }
        }

        [Fact]
        public async Task GetByExternalIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var options = CreateInMemoryOptions();

            using var context = new TransactionDbContext(options);
            var repo = new TransactionRepository(context);

            var result = await repo.GetByExternalIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldChangeStatus_WhenExists()
        {
            var options = CreateInMemoryOptions();
            var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 1, 300m);

            using (var context = new TransactionDbContext(options))
            {
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();
            }

            using (var context = new TransactionDbContext(options))
            {
                var repo = new TransactionRepository(context);
                await repo.UpdateStatusAsync(transaction.TransactionExternalId, TransactionStatus.Approved);
            }

            using (var context = new TransactionDbContext(options))
            {
                var updated = await context.Transactions.FirstOrDefaultAsync();
                Assert.Equal(TransactionStatus.Approved, updated!.Status);
            }
        }

        [Fact]
        public async Task GetDailyAccumulatedValueAsync_ShouldReturnSumOfValuesForDate()
        {
            var options = CreateInMemoryOptions();
            var sourceAccountId = Guid.NewGuid();
            var today = DateTime.UtcNow.Date;

            var t1 = new Transaction(sourceAccountId, Guid.NewGuid(), 1, 500);
            var t2 = new Transaction(sourceAccountId, Guid.NewGuid(), 1, 1000);
            var t3 = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 1, 2000); // otro origen

            using (var context = new TransactionDbContext(options))
            {
                t1.GetType().GetProperty(nameof(Transaction.CreatedAt))!
                    .SetValue(t1, today.AddHours(1));
                t2.GetType().GetProperty(nameof(Transaction.CreatedAt))!
                    .SetValue(t2, today.AddHours(5));
                t3.GetType().GetProperty(nameof(Transaction.CreatedAt))!
                    .SetValue(t3, today.AddHours(2));

                context.Transactions.AddRange(t1, t2, t3);
                await context.SaveChangesAsync();
            }

            using (var context = new TransactionDbContext(options))
            {
                var repo = new TransactionRepository(context);
                var result = await repo.GetDailyAccumulatedValueAsync(sourceAccountId, today);

                Assert.Equal(1500m, result);
            }
        }
    }
}
