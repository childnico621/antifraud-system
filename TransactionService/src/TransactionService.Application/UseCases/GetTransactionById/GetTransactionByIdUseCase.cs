using System;
using System.Threading.Tasks;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Ports;

namespace TransactionService.Application.UseCases.GetTransactionById
{
    public class GetTransactionByIdUseCase : IGetTransactionByIdUseCase
    {
        private readonly ITransactionRepository _repository;

        public GetTransactionByIdUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<TransactionResponseDto?> ExecuteAsync(Guid transactionExternalId)
        {
            var transaction = await _repository.GetByExternalIdAsync(transactionExternalId);

            if (transaction == null)
                return null;

            return new TransactionResponseDto
            {
                TransactionExternalId = transaction.TransactionExternalId,
                CreatedAt = transaction.CreatedAt,
                Status = transaction.Status.ToString()
            };
        }
    }
}
