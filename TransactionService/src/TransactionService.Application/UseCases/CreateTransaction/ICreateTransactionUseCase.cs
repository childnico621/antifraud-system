using System.Threading.Tasks;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.UseCases.CreateTransaction
{
    public interface ICreateTransactionUseCase
    {
        Task<TransactionResponseDto> ExecuteAsync(CreateTransactionCommand command);
    }
}