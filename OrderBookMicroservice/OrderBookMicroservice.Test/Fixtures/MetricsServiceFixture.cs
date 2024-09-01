using OrderBookMicroservice.Application.Services;
using OrderBookMicroservice.Domain.Entities;

namespace OrderBookMicroservice.Test.Fixtures
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
