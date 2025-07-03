using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
            //TODO: pendiente organizar el docker-compose para consumir el servicio con el dominio adecuado ajustar el servicio por el momento manual
            var url = $"http://localhost:5136/transactions/daily-total?sourceAccountId={sourceAccountId}&date={date:yyyy-MM-dd}";
            var rawJson = await _httpClient.GetStringAsync(url);
            Console.WriteLine(rawJson);
            var result = JsonSerializer.Deserialize<DailyTotalResponse>(rawJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Total ?? 0;
        }

        private sealed class DailyTotalResponse
        {
            public decimal Total { get; set; } = 0;
        }
    }
}
