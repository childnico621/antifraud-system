using System;
using System.Threading.Tasks;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.UseCases.GetTransactionById
{
    public interface IGetTransactionByIdUseCase
    {
        Task<TransactionResponseDto?> ExecuteAsync(Guid transactionExternalId);
    }
}