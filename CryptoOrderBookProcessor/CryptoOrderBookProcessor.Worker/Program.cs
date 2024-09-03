using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CryptoOrderBookProcessor.Application.Configurations;
using CryptoOrderBookProcessor.Infrastructure.Configurations;
using CryptoOrderBookProcessor.Worker;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMongoDb("mongodb://localhost:27017", "orderbookdb");
        services.AddApplicationServices();
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();
