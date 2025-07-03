using System;

namespace TransactionService.Application.DTOs
{
    public class TransactionResponseDto
    {
        public Guid TransactionExternalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
