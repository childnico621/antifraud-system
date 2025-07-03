namespace AntiFraudService.Domain.Entities
{
    public enum TransactionStatus
    {
        Approved,
        Rejected
    }

    public class TransactionEvaluationResult
    {
        public TransactionStatus Status { get; private set; }
        public string? Reason { get; private set; }

        private TransactionEvaluationResult(TransactionStatus status, string? reason = null)
        {
            Status = status;
            Reason = reason;
        }


        public static TransactionEvaluationResult Approved()
        {
            return new TransactionEvaluationResult(TransactionStatus.Approved);
        }

        public static TransactionEvaluationResult Rejected(string reason)
        {
            return new TransactionEvaluationResult(TransactionStatus.Rejected, reason);
        }
        
        public bool IsRejected() => Status == TransactionStatus.Rejected;
    }
}