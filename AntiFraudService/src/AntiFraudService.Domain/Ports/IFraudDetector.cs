using System.Threading.Tasks;
using AntiFraudService.Domain.Entities;

namespace AntiFraudService.Domain.Ports
{
    public interface IFraudDetector
    {
        Task<TransactionEvaluationResult> EvaluateAsync(TransactionValidated transaction);
    }
}
