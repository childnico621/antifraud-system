using System.Threading.Tasks;
using AntiFraudService.Application.DTOs;
using AntiFraudService.Domain.Entities;

using AntiFraudService.Domain.Ports;
using AntiFraudService.Domain.Services;

namespace AntiFraudService.Application.UseCases
{
    public class EvaluateTransactionUseCase
    {
        private readonly IFraudDetectorService _fraudDetector;
        private readonly ITransactionCommandPort _commandPort;

        public EvaluateTransactionUseCase(IFraudDetectorService fraudDetector, ITransactionCommandPort commandPort)
        {
            _fraudDetector = fraudDetector;
            _commandPort = commandPort;
        }

        public async Task ExecuteAsync(TransactionCreatedEvent evt)
        {
            var result = await _fraudDetector.EvaluateAsync(evt);

            if (result.IsRejected())
            {
                await _commandPort.UpdateStatusAsync(evt.TransactionExternalId, TransactionStatus.Rejected);
            }
        }
    }
}
