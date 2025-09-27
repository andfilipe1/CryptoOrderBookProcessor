using Xunit;
using CryptoOrderBookProcessor.Test.Fixtures;
using CryptoOrderBookProcessor.Domain.Entities;
using System.Threading.Tasks;
using CryptoOrderBookProcessor.Application.Services;

namespace CryptoOrderBookProcessor.Test.Services
{
    public class MetricsServiceTests
    {
        private readonly MetricsService _metricsService;

        public MetricsServiceTests()
        {
            _metricsService = new MetricsService();
        }

        [Fact]
        public void ProcessOrderBookData_Should_Add_BtcPrices_And_Quantities()
        {
            // Arrange
            var bids = new List<Order>
            {
                new Order { Price = 30000, Quantity = 0.5m },
                new Order { Price = 31000, Quantity = 0.7m }
            };
            var asks = new List<Order>(); // empty asks for this test
            var orderBook = MetricsServiceFixture.CreateOrderBook("btcusd", bids, asks);

            // Act
            _metricsService.ProcessOrderBookData(orderBook);  
        }

        [Fact]
        public async Task CalculateMetricsEvery5Seconds_Should_Clear_BtcLists_After_Calculation()
        {
            // Arrange
            var bids = new List<Order>
                {
                    new() { Price = 30000, Quantity = 0.5m },
                    new() { Price = 31000, Quantity = 0.7m }
                };
            bool taskCanceled = false;

            var asks = new List<Order>();
            var orderBook = MetricsServiceFixture.CreateOrderBook("btcusd", bids, asks);
            _metricsService.ProcessOrderBookData(orderBook);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var task = _metricsService.CalculateMetricsEvery5Seconds(cancellationTokenSource.Token);

            await Task.Delay(5500);

            cancellationTokenSource.Cancel();

            try
            {
                await task;
            }
            catch (TaskCanceledException)
            {
                taskCanceled = true;
            }

            // Assert
            var metrics = _metricsService.GetMetricsSnapshot();
            Assert.Equal(0, metrics.BtcPricesCount);
            Assert.Equal(0, metrics.BtcQuantitiesCount);
            Assert.True(taskCanceled, "A task deveria ter sido cancelada.");
        }

        [Fact]
        public void ProcessOrderBookData_Should_Add_EthPrices_And_Quantities()
        {
            // Arrange
            var asks = new List<Order>
            {
                new Order { Price = 2000, Quantity = 1m },
                new Order { Price = 2100, Quantity = 1.2m }
            };
            var bids = new List<Order>(); // empty bids for this test
            var orderBook = MetricsServiceFixture.CreateOrderBook("ethusd", bids, asks);

            // Act
            _metricsService.ProcessOrderBookData(orderBook);

            // Assert - With our fix, ETH now processes both bids and asks
            var snapshot = _metricsService.GetMetricsSnapshot();
            Assert.Equal(2, snapshot.EthPricesCount);
            Assert.Equal(2, snapshot.EthQuantitiesCount);
        }

        [Fact]
        public async Task CalculateMetricsEvery5Seconds_Should_Handle_Cancellation_Gracefully()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var task = _metricsService.CalculateMetricsEvery5Seconds(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            // Assert
            var exception = await Record.ExceptionAsync(() => task);
            Assert.IsType<TaskCanceledException>(exception);   
        }

        [Fact]
        public void ProcessOrderBookData_Should_Ignore_Unknown_Instrument()
        {
            // Arrange
            var bids = new List<Order>
            {
                new Order { Price = 100, Quantity = 0.5m }
            };
            var asks = new List<Order>
            {
                new Order { Price = 105, Quantity = 0.5m }
            };
            var orderBook = MetricsServiceFixture.CreateOrderBook("unknownusd", bids, asks);

            // Act
            _metricsService.ProcessOrderBookData(orderBook);

            // Assert - Unknown instruments should not affect BTC or ETH counts
            var snapshot = _metricsService.GetMetricsSnapshot();
            Assert.Equal(0, snapshot.BtcPricesCount);
            Assert.Equal(0, snapshot.EthPricesCount);
        }

        [Fact]
        public void ProcessOrderBookData_Should_Process_Both_Bids_And_Asks_For_BTC()
        {
            // Arrange
            var bids = new List<Order>
            {
                new Order { Price = 30000, Quantity = 0.5m }
            };
            var asks = new List<Order>
            {
                new Order { Price = 30100, Quantity = 0.6m }
            };
            var orderBook = MetricsServiceFixture.CreateOrderBook("btcusd", bids, asks);

            // Act
            _metricsService.ProcessOrderBookData(orderBook);

            // Assert - BTC should process both bids and asks (total: 2 prices, 2 quantities)
            var snapshot = _metricsService.GetMetricsSnapshot();
            Assert.Equal(2, snapshot.BtcPricesCount);
            Assert.Equal(2, snapshot.BtcQuantitiesCount);
        }

        [Fact]
        public void ProcessOrderBookData_Should_Process_Both_Bids_And_Asks_For_ETH()
        {
            // Arrange
            var bids = new List<Order>
            {
                new Order { Price = 2000, Quantity = 1.0m }
            };
            var asks = new List<Order>
            {
                new Order { Price = 2100, Quantity = 1.5m }
            };
            var orderBook = MetricsServiceFixture.CreateOrderBook("ethusd", bids, asks);

            // Act
            _metricsService.ProcessOrderBookData(orderBook);

            // Assert - ETH should process both bids and asks (total: 2 prices, 2 quantities)
            var snapshot = _metricsService.GetMetricsSnapshot();
            Assert.Equal(2, snapshot.EthPricesCount);
            Assert.Equal(2, snapshot.EthQuantitiesCount);
        }
    }
}
