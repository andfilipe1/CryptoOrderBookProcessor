using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderBookMicroservice.Application.Configurations;
using OrderBookMicroservice.Infrastructure.Configurations;
using OrderBookMicroservice.Worker;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMongoDb("mongodb://localhost:27017", "orderbookdb");
        services.AddApplicationServices();
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();
