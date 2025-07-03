using System;

namespace AntiFraudService.Domain.Entities
{
    public class TransactionValidated
    {
        public Guid TransactionExternalId { get; set; }
        public Guid SourceAccountId { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
