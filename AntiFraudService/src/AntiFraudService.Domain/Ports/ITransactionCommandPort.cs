using System;
using System.Threading.Tasks;
using AntiFraudService.Domain.Entities;

namespace AntiFraudService.Domain.Ports
{
    public interface ITransactionCommandPort
    {
        Task UpdateStatusAsync(Guid transactionExternalId, TransactionStatus newStatus);
    }
}
