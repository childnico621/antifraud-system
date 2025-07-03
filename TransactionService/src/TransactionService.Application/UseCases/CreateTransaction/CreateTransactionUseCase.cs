using System.Threading.Tasks;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Ports;

namespace TransactionService.Application.UseCases.CreateTransaction
{
    public class CreateTransactionUseCase : ICreateTransactionUseCase
    {
        private readonly ITransactionRepository _repository;
        private readonly IEventPublisher _eventPublisher;

        public CreateTransactionUseCase(ITransactionRepository repository, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<TransactionResponseDto> ExecuteAsync(CreateTransactionCommand command)
        {
            var transaction = new Transaction(
                command.SourceAccountId,
                command.TargetAccountId,
                command.TransferTypeId,
                command.Value
            );

            await _repository.CreateAsync(transaction);
            await _eventPublisher.PublishTransactionCreatedAsync(transaction);

            return new TransactionResponseDto
            {
                TransactionExternalId = transaction.TransactionExternalId,
                CreatedAt = transaction.CreatedAt,
                Status = transaction.Status.ToString()
            };
        }
    }
}
