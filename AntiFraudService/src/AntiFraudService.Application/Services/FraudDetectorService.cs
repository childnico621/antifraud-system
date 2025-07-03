using System;
using System.Threading.Tasks;
using AntiFraudService.Application.DTOs;
using AntiFraudService.Domain.Entities;
using AntiFraudService.Domain.Ports;
using AntiFraudService.Domain.Services;

namespace AntiFraudService.Application.Services
{
    public class FraudDetectorService : IFraudDetectorService
    {
        private readonly ITransactionQueryPort _transactionQueryPort;

        public FraudDetectorService(ITransactionQueryPort transactionQueryPort)
        {
            _transactionQueryPort = transactionQueryPort;
        }

        public async Task<TransactionEvaluationResult> EvaluateAsync(TransactionCreatedEvent transaction)
        {
            if (transaction.Value > 2000)
            {
                return TransactionEvaluationResult.Rejected("Transaction value exceeds limit");
            }

            var dailyTotal = await _transactionQueryPort.GetDailyAccumulatedValueAsync(
                transaction.SourceAccountId,
                transaction.CreatedAt
            );

            if (dailyTotal + transaction.Value > 20000)
            {
                return TransactionEvaluationResult.Rejected("Daily transaction limit exceeded");
            }

            return TransactionEvaluationResult.Approved();
        }
    }
}
