using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntiFraudService.Domain.Ports;

namespace AntiFraudService.Infrastructure.Adapters
{
    public class TransactionQueryAdapter : ITransactionQueryPort
    {
        private readonly HttpClient _httpClient;

        public TransactionQueryAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetDailyAccumulatedValueAsync(Guid sourceAccountId, DateTime date)
        {
            var url = $"http://transaction-service/transactions/daily-total?sourceAccountId={sourceAccountId}&date={date:yyyy-MM-dd}";
            var result = await _httpClient.GetFromJsonAsync<DailyTotalResponse>(url);

            return result?.Total ?? 0;
        }

        private class DailyTotalResponse
        {
            public decimal Total { get; set; }
        }
    }
}
