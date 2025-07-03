using System;
using System.Threading.Tasks;
using AntiFraudService.Application.DTOs;
using AntiFraudService.Domain.Entities;

namespace AntiFraudService.Domain.Services
{
    public interface IFraudDetectorService
    {
        Task<TransactionEvaluationResult> EvaluateAsync(TransactionCreatedEvent transaction);
    }
}