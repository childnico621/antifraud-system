using TransactionService.Application.UseCases.CreateTransaction;
using TransactionService.Application.UseCases.GetTransactionById;
using TransactionService.Domain.Ports;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Confluent.Kafka;
using TransactionService.Application.UseCases.UpdateTransactionStatus;

var builder = WebApplication.CreateBuilder(args);

// Agrega los casos de uso
builder.Services.AddScoped<ICreateTransactionUseCase, CreateTransactionUseCase>();
builder.Services.AddScoped<IGetTransactionByIdUseCase, GetTransactionByIdUseCase>();
builder.Services.AddScoped<IUpdateTransactionStatusUseCase, UpdateTransactionStatusUseCase>();

// Puerto de salida (infraestructura)
builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var kafka = builder.Configuration.GetSection("Kafka");
    var config = new ProducerConfig
    {
        BootstrapServers = $"{kafka.GetValue<string>("BootstrapServers")}:{kafka.GetValue<string>("Port")}" ?? "localhost:9092"
    };

    return new ProducerBuilder<Null, string>(config).Build();
});

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IEventPublisher, KafkaEventPublisher>();

builder.Services.AddDbContext<TransactionDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
    await db.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


await app.RunAsync();
