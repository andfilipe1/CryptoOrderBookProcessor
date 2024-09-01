using Microsoft.Extensions.DependencyInjection;
using OrderBookMicroservice.Application.Interfaces;
using OrderBookMicroservice.Application.Services;
using OrderBookMicroservice.Application.UseCases;

namespace OrderBookMicroservice.Application.Configurations
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
