using CryptoOrderBookProcessor.Application.Interfaces;
using CryptoOrderBookProcessor.Domain.Entities;

namespace CryptoOrderBookProcessor.Application.Services
{
    public class MetricsService : IMetricsService
    {
        private readonly List<decimal> _btcPrices = [];
        private readonly List<decimal> _ethPrices = [];
        private readonly List<decimal> _btcQuantities = [];
        private readonly List<decimal> _ethQuantities = [];

        public void ProcessOrderBookData(OrderBook orderBook)
        {
            if (orderBook.Instrument == "btcusd")
            {
                foreach (var bid in orderBook.Bids)
                {
                    _btcPrices.Add(bid.Price);
                    _btcQuantities.Add(bid.Quantity);
                }
            }
            else if (orderBook.Instrument == "ethusd")
            {
                foreach (var ask in orderBook.Asks)
                {
                    _ethPrices.Add(ask.Price);
                    _ethQuantities.Add(ask.Quantity);
                }
            }
        }

        public async Task CalculateMetricsEvery5Seconds(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);

                // Calcular métricas para BTC/USD
                if (_btcPrices.Count > 0)
                {
                    Console.WriteLine($"BTC/USD -> Max Price: {_btcPrices.Max()}, Min Price: {_btcPrices.Min()}, Avg Price: {_btcPrices.Average():F2}, Avg Quantity: {_btcQuantities.Average():F2}");
                    _btcPrices.Clear();
                    _btcQuantities.Clear();
                }

                // Calcular métricas para ETH/USD
                if (_ethPrices.Count > 0)
                {
                    Console.WriteLine($"ETH/USD -> Max Price: {_ethPrices.Max()}, Min Price: {_ethPrices.Min()}, Avg Price: {_ethPrices.Average():F2}, Avg Quantity: {_ethQuantities.Average():F2}");
                    _ethPrices.Clear();
                    _ethQuantities.Clear();
                }
            }
        }

        public MetricsSnapshot GetMetricsSnapshot()
        {
            return new MetricsSnapshot
            {
                BtcPricesCount = _btcPrices.Count,
                BtcQuantitiesCount = _btcQuantities.Count,
                EthPricesCount = _ethPrices.Count,
                EthQuantitiesCount = _ethQuantities.Count
            };
        }
 
    }
}
