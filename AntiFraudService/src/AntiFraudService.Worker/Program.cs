using AntiFraudService.Application.Services;
using AntiFraudService.Application.UseCases;
using AntiFraudService.Domain.Ports;
using AntiFraudService.Domain.Services;
using AntiFraudService.Infrastructure.Adapters;
using AntiFraudService.Worker;
using AntiFraudService.Worker.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<KafkaTransactionConsumer>();


builder.Services.AddScoped<EvaluateTransactionUseCase>();
builder.Services.AddScoped<IFraudDetectorService, FraudDetectorService>();
builder.Services.AddHttpClient<ITransactionQueryPort, TransactionQueryAdapter>();
builder.Services.AddScoped<ITransactionCommandPort, TransactionCommandAdapter>();


builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
