using System.Net.Http;
using System.Text;
using System.Text.Json;
using AntiFraudService.Domain.Entities;
using AntiFraudService.Domain.Ports;

namespace AntiFraudService.Infrastructure.Adapters
{
    public class TransactionCommandAdapter : ITransactionCommandPort
    {
        private readonly HttpClient _httpClient;

        public TransactionCommandAdapter()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
        }

        public async Task UpdateStatusAsync(Guid transactionExternalId, TransactionStatus newStatus)
        {
            var payload = new
            {
                Status = newStatus.ToString()
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/transactions/{transactionExternalId}/status", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error updating status: {response.StatusCode} - {error}");
            }
        }
    }
}
