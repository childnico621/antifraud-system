using System;
using System.Threading.Tasks;

namespace TransactionService.Application.UseCases.GetDailyTotal
{
    public interface IGetDailyTotalUseCase
    {
        Task<decimal> ExecuteAsync(Guid sourceAccountId, DateTime date);
    }
}
