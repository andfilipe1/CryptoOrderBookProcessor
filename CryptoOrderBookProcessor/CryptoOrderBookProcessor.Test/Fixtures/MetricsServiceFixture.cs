using CryptoOrderBookProcessor.Application.Services;
using CryptoOrderBookProcessor.Domain.Entities;

namespace CryptoOrderBookProcessor.Test.Fixtures
{
    public class MetricsServiceFixture 
    {
        public MetricsService MetricsService { get; private set; }

        public MetricsServiceFixture()
        {
            MetricsService = new MetricsService();
        }

        public static OrderBook CreateOrderBook(string instrument, List<Order> bids, List<Order> asks)
        {
            return new OrderBook
            {
                Instrument = instrument,
                Bids = bids,
                Asks = asks
            };
        }
    }
}
