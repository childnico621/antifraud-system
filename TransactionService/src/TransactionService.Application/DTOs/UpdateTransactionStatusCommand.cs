namespace TransactionService.Application.DTOs
{
    public class UpdateTransactionStatusCommand
    {
        public Guid TransactionExternalId { get; set; }
        public string Status { get; set; } = null!;
    }
}