using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Moq;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Messaging;
using Xunit;

namespace TransactionService.UnitTests.Infrastructure.Messaging
{
    public class KafkaEventPublisherTests
    {
        [Fact]
        public async Task PublishTransactionCreatedAsync_SendsSerializedMessageToKafka()
        {
            // Arrange
            var transaction = new Transaction(
                Guid.NewGuid(),
                Guid.NewGuid(),
                1,
                1000
            );

            var mockProducer = new Mock<IProducer<Null, string>>();

            // Simula que ProduceAsync devuelve una Task completada exitosamente
            mockProducer
                .Setup(p => p.ProduceAsync(
                    It.IsAny<string>(),
                    It.IsAny<Message<Null, string>>(),
                    default))
                .ReturnsAsync(new DeliveryResult<Null, string>());

            var publisher = new KafkaEventPublisher(mockProducer.Object, "transactions-topic");

            // Act
            await publisher.PublishTransactionCreatedAsync(transaction);

            // Assert
            mockProducer.Verify(p =>
                p.ProduceAsync(
                    "transactions-topic",
                    It.Is<Message<Null, string>>(m =>
                        m.Value.Contains(transaction.TransactionExternalId.ToString()) &&
                        m.Value.Contains(transaction.Status.ToString()) &&
                        m.Value.Contains(transaction.CreatedAt.ToString("yyyy-MM-dd"))),
                    default),
                Times.Once);
        }
    }
}
