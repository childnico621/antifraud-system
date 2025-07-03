using System.Text.Json;
using Confluent.Kafka;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Ports;

namespace TransactionService.Infrastructure.Messaging
{
    public class KafkaEventPublisher : IEventPublisher
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaEventPublisher(IProducer<Null, string> producer, string topic = "transaction_created")
        {
            _producer = producer;
            _topic = topic;
        }


        public async Task PublishTransactionCreatedAsync(Transaction transaction)
        {
            var payload = new
            {
                transaction.TransactionExternalId,
                transaction.SourceAccountId,
                transaction.TargetAccountId,
                transaction.TransferTypeId,
                transaction.Value,
                transaction.Status,
                transaction.CreatedAt
            };

            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(payload)
            };

            await _producer.ProduceAsync(_topic, message);
        }
    }
}
