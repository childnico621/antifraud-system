using TransactionService.Application.DTOs;
using TransactionService.Domain.Ports;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.UseCases.UpdateTransactionStatus
{
    public class UpdateTransactionStatusUseCase: IUpdateTransactionStatusUseCase
    {
        private readonly ITransactionRepository _repository;

        public UpdateTransactionStatusUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecuteAsync(UpdateTransactionStatusCommand command)
        {
            if (!Enum.TryParse<TransactionStatus>(command.Status, out var newStatus))
                return false;

            await _repository.UpdateStatusAsync(command.TransactionExternalId, newStatus);
            return true;
        }
    }
}