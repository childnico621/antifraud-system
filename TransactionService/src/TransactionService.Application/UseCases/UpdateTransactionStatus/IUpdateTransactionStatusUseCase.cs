using System.Threading.Tasks;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.UseCases.UpdateTransactionStatus
{
    public interface IUpdateTransactionStatusUseCase
    {
        Task<bool> ExecuteAsync(UpdateTransactionStatusCommand command);
    }
}