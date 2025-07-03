using System.Text.Json;
using Confluent.Kafka;
using AntiFraudService.Application.DTOs;
using AntiFraudService.Application.UseCases;

namespace AntiFraudService.Worker.Messaging
{
    public class KafkaTransactionConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _topic = "transaction_created";
        private readonly string _bootstrapServers = "localhost:9092";

        public KafkaTransactionConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = "antifraud-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    var message = result.Message.Value;

                    var transaction = JsonSerializer.Deserialize<TransactionCreatedEvent>(message, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (transaction != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var useCase = scope.ServiceProvider.GetRequiredService<EvaluateTransactionUseCase>();

                        await useCase.ExecuteAsync(transaction);
                        Console.WriteLine($"Evaluated transaction {transaction.TransactionExternalId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error consuming or processing message: {ex.Message}");
                }
            }

            consumer.Close();
        }
    }
}
