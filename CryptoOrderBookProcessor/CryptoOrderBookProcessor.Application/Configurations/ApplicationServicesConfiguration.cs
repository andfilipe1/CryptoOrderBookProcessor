using Microsoft.Extensions.DependencyInjection;
using CryptoOrderBookProcessor.Application.Interfaces;
using CryptoOrderBookProcessor.Application.Services;
using CryptoOrderBookProcessor.Application.UseCases;

namespace CryptoOrderBookProcessor.Application.Configurations
{
    public static class ApplicationServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Registrar os casos de uso
            services.AddTransient<SaveOrderBookData>();

            // Registrar os serviços de aplicação
            services.AddSingleton<IWebSocketService, WebSocketService>();
            services.AddSingleton<IOrderBookProcessingService, OrderBookProcessingService>();
            services.AddSingleton<IMetricsService, MetricsService>();
        }
    }
}
