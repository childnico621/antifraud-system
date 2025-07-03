using System;
using System.Threading.Tasks;
using Moq;
using TransactionService.Application.DTOs;
using TransactionService.Application.UseCases.GetTransactionById;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Ports;
using Xunit;

namespace TransactionService.UnitTests.Application
{
    public class GetTransactionByIdUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldReturnTransactionResponseDto_WhenTransactionExists()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 1, 1500m);
            
            // Forzamos el mismo TransactionExternalId para que coincida con la búsqueda
            typeof(Transaction).GetProperty(nameof(Transaction.TransactionExternalId))!
                .SetValue(transaction, externalId);

            var mockRepo = new Mock<ITransactionRepository>();
            mockRepo.Setup(r => r.GetByExternalIdAsync(externalId))
                    .ReturnsAsync(transaction);

            var useCase = new GetTransactionByIdUseCase(mockRepo.Object);

            // Act
            var result = await useCase.ExecuteAsync(externalId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transaction.TransactionExternalId, result!.TransactionExternalId);
            Assert.Equal(transaction.Status.ToString(), result.Status);
            Assert.Equal(transaction.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNull_WhenTransactionDoesNotExist()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var mockRepo = new Mock<ITransactionRepository>();
            mockRepo.Setup(r => r.GetByExternalIdAsync(externalId))
                    .ReturnsAsync((Transaction?)null);

            var useCase = new GetTransactionByIdUseCase(mockRepo.Object);

            // Act
            var result = await useCase.ExecuteAsync(externalId);

            // Assert
            Assert.Null(result);
        }
    }
}
