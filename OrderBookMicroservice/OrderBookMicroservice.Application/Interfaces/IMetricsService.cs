using OrderBookMicroservice.Domain.Entities;

namespace OrderBookMicroservice.Application.Interfaces
{
    public interface IMetricsService
    {
        void ProcessOrderBookData(OrderBook orderBook);
        Task CalculateMetricsEvery5Seconds(CancellationToken stoppingToken);
    }
}
