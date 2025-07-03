using System;

namespace TransactionService.Application.DTOs
{
    public class CreateTransactionCommand
    {
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public decimal Value { get; set; }
    }
}
