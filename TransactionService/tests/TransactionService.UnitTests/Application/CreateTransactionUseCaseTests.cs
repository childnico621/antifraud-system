using System;
using System.Threading.Tasks;
using Moq;
using TransactionService.Application.DTOs;
using TransactionService.Application.UseCases.CreateTransaction;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Ports;
using Xunit;

namespace TransactionService.UnitTests.Application
{
    public class CreateTransactionUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldCreateTransaction_AndReturnResponseDto()
        {
            // Arrange
            var command = new CreateTransactionCommand
            {
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferTypeId = 1,
                Value = 1000m
            };

            var mockRepo = new Mock<ITransactionRepository>();
            var mockPublisher = new Mock<IEventPublisher>();

            // Simula que CreateAsync no lanza errores
            mockRepo.Setup(r => r.CreateAsync(It.IsAny<Transaction>()))
                    .Returns(Task.CompletedTask);

            mockPublisher.Setup(p => p.PublishTransactionCreatedAsync(It.IsAny<Transaction>()))
                         .Returns(Task.CompletedTask);

            var useCase = new CreateTransactionUseCase(mockRepo.Object, mockPublisher.Object);

            // Act
            var result = await useCase.ExecuteAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.TransactionExternalId);
            Assert.False(string.IsNullOrWhiteSpace(result.Status));
            Assert.True((DateTime.UtcNow - result.CreatedAt).TotalSeconds < 5);

            mockRepo.Verify(r => r.CreateAsync(It.IsAny<Transaction>()), Times.Once);
            mockPublisher.Verify(p => p.PublishTransactionCreatedAsync(It.IsAny<Transaction>()), Times.Once);
        }
    }
}
