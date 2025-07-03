using System;
using TransactionService.Domain.Entities;
using Xunit;

namespace TransactionService.UnitTests.Domain
{
    public class TransactionTests
    {
        [Fact]
        public void Constructor_ShouldInitializeAllPropertiesCorrectly()
        {
            // Arrange
            var sourceAccountId = Guid.NewGuid();
            var targetAccountId = Guid.NewGuid();
            var transferTypeId = 1;
            var value = 500.00m;

            // Act
            var transaction = new Transaction(sourceAccountId, targetAccountId, transferTypeId, value);

            // Assert
            Assert.NotEqual(Guid.Empty, transaction.Id);
            Assert.NotEqual(Guid.Empty, transaction.TransactionExternalId);
            Assert.Equal(sourceAccountId, transaction.SourceAccountId);
            Assert.Equal(targetAccountId, transaction.TargetAccountId);
            Assert.Equal(transferTypeId, transaction.TransferTypeId);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(TransactionStatus.Pending, transaction.Status);
            Assert.True((DateTime.UtcNow - transaction.CreatedAt).TotalSeconds < 5);
        }

        [Fact]
        public void UpdateStatus_ShouldChangeTransactionStatus()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 1, 1000);
            
            // Act
            transaction.UpdateStatus(TransactionStatus.Approved);

            // Assert
            Assert.Equal(TransactionStatus.Approved, transaction.Status);
        }
    }
}
