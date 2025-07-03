using System;

namespace TransactionService.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid TransactionExternalId { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public decimal Value { get; set; }
        public TransactionStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Transaction(Guid sourceAccountId, Guid targetAccountId, int transferTypeId, decimal value)
        {
            Id = Guid.NewGuid();
            TransactionExternalId = Guid.NewGuid();
            SourceAccountId = sourceAccountId;
            TargetAccountId = targetAccountId;
            TransferTypeId = transferTypeId;
            Value = value;
            Status = TransactionStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(TransactionStatus status)
        {
            Status = status;
        }
    }
}


