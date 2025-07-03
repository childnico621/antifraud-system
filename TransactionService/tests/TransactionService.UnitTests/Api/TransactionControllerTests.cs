using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TransactionService.Api.Controllers;
using TransactionService.Application.DTOs;
using TransactionService.Application.UseCases.CreateTransaction;
using TransactionService.Application.UseCases.GetTransactionById;
using Xunit;

namespace TransactionService.UnitTests.Api
{
    public class TransactionControllerTests
    {
        [Fact]
        public async Task CreateTransaction_ShouldReturnCreatedResult()
        {
            // Arrange
            var command = new CreateTransactionCommand
            {
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferTypeId = 1,
                Value = 1000
            };

            var responseDto = new TransactionResponseDto
            {
                TransactionExternalId = Guid.NewGuid(),
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            var mockCreate = new Mock<ICreateTransactionUseCase>();
            mockCreate.Setup(c => c.ExecuteAsync(command))
                      .ReturnsAsync(responseDto);

            var mockGet = new Mock<IGetTransactionByIdUseCase>();

            var controller = new TransactionController(mockCreate.Object, mockGet.Object);

            // Act
            var result = await controller.CreateTransaction(command);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedDto = Assert.IsType<TransactionResponseDto>(createdResult.Value);
            Assert.Equal(responseDto.TransactionExternalId, returnedDto.TransactionExternalId);
        }

        [Fact]
        public async Task GetTransaction_ShouldReturnOk_WhenFound()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var responseDto = new TransactionResponseDto
            {
                TransactionExternalId = transactionId,
                Status = "Completed",
                CreatedAt = DateTime.UtcNow
            };

            var mockGet = new Mock<IGetTransactionByIdUseCase>();
            mockGet.Setup(g => g.ExecuteAsync(transactionId))
                   .ReturnsAsync(responseDto);

            var mockCreate = new Mock<ICreateTransactionUseCase>();

            var controller = new TransactionController(mockCreate.Object, mockGet.Object);

            // Act
            var result = await controller.GetTransaction(transactionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDto = Assert.IsType<TransactionResponseDto>(okResult.Value);
            Assert.Equal(transactionId, returnedDto.TransactionExternalId);
        }

        [Fact]
        public async Task GetTransaction_ShouldReturnNotFound_WhenMissing()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var mockGet = new Mock<IGetTransactionByIdUseCase>();
            mockGet.Setup(g => g.ExecuteAsync(transactionId))
                   .ReturnsAsync((TransactionResponseDto?)null);

            var mockCreate = new Mock<ICreateTransactionUseCase>();

            var controller = new TransactionController(mockCreate.Object, mockGet.Object);

            // Act
            var result = await controller.GetTransaction(transactionId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
