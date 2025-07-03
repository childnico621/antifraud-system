using System;
using AntiFraudService.Domain.Entities;

namespace AntiFraudService.Application.DTOs
{
    public class TransactionCreatedEvent
    {
        public Guid TransactionExternalId { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public decimal Value { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
