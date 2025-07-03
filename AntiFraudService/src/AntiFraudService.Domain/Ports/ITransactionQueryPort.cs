using System;
using System.Threading.Tasks;

namespace AntiFraudService.Domain.Ports
{
    public interface ITransactionQueryPort
    {
        Task<decimal> GetDailyAccumulatedValueAsync(Guid sourceAccountId, DateTime date);
    }
}
