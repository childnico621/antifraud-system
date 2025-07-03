using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Ports;

namespace TransactionService.Infrastructure.Persistence
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _context;

        public TransactionRepository(TransactionDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction?> GetByExternalIdAsync(Guid transactionExternalId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TransactionExternalId == transactionExternalId);
        }

        public async Task UpdateStatusAsync(Guid transactionExternalId, TransactionStatus newStatus)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionExternalId == transactionExternalId);
            if (transaction is null) return;

            transaction.UpdateStatus(newStatus);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetDailyAccumulatedValueAsync(Guid sourceAccountId, DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await _context.Transactions
                .Where(t => t.SourceAccountId == sourceAccountId && t.CreatedAt >= start && t.CreatedAt < end)
                .SumAsync(t => t.Value);
        }
    }
}
