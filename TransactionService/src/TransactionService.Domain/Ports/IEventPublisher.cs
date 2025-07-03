using System.Threading.Tasks;
using TransactionService.Domain.Entities;

namespace TransactionService.Domain.Ports
{
    public interface IEventPublisher
    {
        Task PublishTransactionCreatedAsync(Transaction transaction);
    }
}
