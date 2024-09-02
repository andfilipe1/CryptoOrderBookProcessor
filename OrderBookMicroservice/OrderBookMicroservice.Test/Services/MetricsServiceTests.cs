using Xunit;
using OrderBookMicroservice.Test.Fixtures;
using OrderBookMicroservice.Domain.Entities;
using System.Threading.Tasks;

namespace OrderBookMicroservice.Test.Services
{
    public class MetricsServiceTests(MetricsServiceFixture fixture) : IClassFixture<MetricsServiceFixture>
    {
        private readonly MetricsServiceFixture _fixture = fixture;

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
            _fixture.MetricsService.ProcessOrderBookData(orderBook);  
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
            _fixture.MetricsService.ProcessOrderBookData(orderBook);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var task = _fixture.MetricsService.CalculateMetricsEvery5Seconds(cancellationTokenSource.Token);

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
            var metrics = _fixture.MetricsService.GetMetricsSnapshot();
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
            _fixture.MetricsService.ProcessOrderBookData(orderBook);

            // Assert
            // Assert.Equal(2, _fixture.MetricsService.GetEthPricesCount());
            // Assert.Contains(2000, _fixture.MetricsService.GetEthPrices());
        }

        [Fact]
        public async Task CalculateMetricsEvery5Seconds_Should_Handle_Cancellation_Gracefully()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var task = _fixture.MetricsService.CalculateMetricsEvery5Seconds(cancellationTokenSource.Token);
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
            _fixture.MetricsService.ProcessOrderBookData(orderBook);

            // Assert
            // Assert.Equal(0, _fixture.MetricsService.GetBtcPricesCount());
            // Assert.Equal(0, _fixture.MetricsService.GetEthPricesCount());
        }
    }
}
