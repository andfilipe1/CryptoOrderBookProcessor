using CryptoOrderBookProcessor.Domain.Entities;

namespace CryptoOrderBookProcessor.Application.Interfaces
{
    public interface IMetricsService
    {
        void ProcessOrderBookData(OrderBook orderBook);
        Task CalculateMetricsEvery5Seconds(CancellationToken stoppingToken);
    }
}
