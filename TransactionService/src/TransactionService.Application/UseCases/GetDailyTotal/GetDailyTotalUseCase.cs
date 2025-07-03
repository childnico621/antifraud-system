using TransactionService.Domain.Ports;

namespace TransactionService.Application.UseCases.GetDailyTotal
{
    public class GetDailyTotalUseCase: IGetDailyTotalUseCase
    {
        private readonly ITransactionRepository _repository;

        public GetDailyTotalUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<decimal> ExecuteAsync(Guid sourceAccountId, DateTime date)
        {
            return await _repository.GetDailyAccumulatedValueAsync(sourceAccountId, date);
        }
    }
}
