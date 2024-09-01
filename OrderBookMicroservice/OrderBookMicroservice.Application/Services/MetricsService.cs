using OrderBookMicroservice.Application.Interfaces;
using OrderBookMicroservice.Domain.Entities;

public class MetricsService : IMetricsService
{
    private readonly List<decimal> _btcPrices = new();
    private readonly List<decimal> _ethPrices = new();
    private readonly List<decimal> _btcQuantities = new();
    private readonly List<decimal> _ethQuantities = new();

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
}
