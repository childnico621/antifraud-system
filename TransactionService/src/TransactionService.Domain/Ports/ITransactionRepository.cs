using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionService.Domain.Entities;

namespace TransactionService.Domain.Ports
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByExternalIdAsync(Guid transactionExternalId);
        Task CreateAsync(Transaction transaction);
        Task UpdateStatusAsync(Guid transactionExternalId, TransactionStatus newStatus);
        Task<decimal> GetDailyAccumulatedValueAsync(Guid sourceAccountId, DateTime date);
    }
}
