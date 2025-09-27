using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using CryptoOrderBookProcessor.Application.Services;
using CryptoOrderBookProcessor.Domain.Entities;
using System.Text.Json;

namespace CryptoOrderBookProcessor.Test.Services
{
    public class OrderBookProcessingServiceTests
    {
        private readonly Mock<ILogger<OrderBookProcessingService>> _loggerMock;
        private readonly OrderBookProcessingService _service;

        public OrderBookProcessingServiceTests()
        {
            _loggerMock = new Mock<ILogger<OrderBookProcessingService>>();
            _service = new OrderBookProcessingService(_loggerMock.Object);
        }

        [Fact]
        public void ProcessMessage_Should_Return_Null_For_Subscription_Succeeded_Event()
        {
            // Arrange
            var message = @"{""event"": ""bts:subscription_succeeded""}";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ProcessMessage_Should_Return_Null_When_Data_Property_Missing()
        {
            // Arrange
            var message = @"{""channel"": ""order_book_btcusd""}";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ProcessMessage_Should_Parse_BTC_OrderBook_With_Bids_And_Asks()
        {
            // Arrange
            var message = @"{
                ""channel"": ""order_book_btcusd"",
                ""data"": {
                    ""bids"": [
                        [""30000.00"", ""0.50""],
                        [""29900.00"", ""0.75""]
                    ],
                    ""asks"": [
                        [""30100.00"", ""0.60""],
                        [""30200.00"", ""0.80""]
                    ]
                }
            }";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("btcusd", result.Instrument);
            Assert.Equal(2, result.Bids.Count);
            Assert.Equal(2, result.Asks.Count);
            
            // Verify bids
            Assert.Equal(30000.00m, result.Bids[0].Price);
            Assert.Equal(0.50m, result.Bids[0].Quantity);
            Assert.Equal(29900.00m, result.Bids[1].Price);
            Assert.Equal(0.75m, result.Bids[1].Quantity);
            
            // Verify asks
            Assert.Equal(30100.00m, result.Asks[0].Price);
            Assert.Equal(0.60m, result.Asks[0].Quantity);
            Assert.Equal(30200.00m, result.Asks[1].Price);
            Assert.Equal(0.80m, result.Asks[1].Quantity);
        }

        [Fact]
        public void ProcessMessage_Should_Parse_ETH_OrderBook_With_Bids_And_Asks()
        {
            // Arrange
            var message = @"{
                ""channel"": ""order_book_ethusd"",
                ""data"": {
                    ""bids"": [
                        [""2000.00"", ""1.50""]
                    ],
                    ""asks"": [
                        [""2100.00"", ""2.00""]
                    ]
                }
            }";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ethusd", result.Instrument);
            Assert.Single(result.Bids);
            Assert.Single(result.Asks);
            Assert.Equal(2000.00m, result.Bids[0].Price);
            Assert.Equal(1.50m, result.Bids[0].Quantity);
            Assert.Equal(2100.00m, result.Asks[0].Price);
            Assert.Equal(2.00m, result.Asks[0].Quantity);
        }

        [Fact]
        public void ProcessMessage_Should_Handle_Missing_Bids()
        {
            // Arrange
            var message = @"{
                ""channel"": ""order_book_btcusd"",
                ""data"": {
                    ""asks"": [
                        [""30100.00"", ""0.60""]
                    ]
                }
            }";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("btcusd", result.Instrument);
            Assert.Empty(result.Bids);
            Assert.Single(result.Asks);
            Assert.Equal(30100.00m, result.Asks[0].Price);
        }

        [Fact]
        public void ProcessMessage_Should_Handle_Missing_Asks()
        {
            // Arrange
            var message = @"{
                ""channel"": ""order_book_btcusd"",
                ""data"": {
                    ""bids"": [
                        [""30000.00"", ""0.50""]
                    ]
                }
            }";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("btcusd", result.Instrument);
            Assert.Single(result.Bids);
            Assert.Empty(result.Asks);
            Assert.Equal(30000.00m, result.Bids[0].Price);
        }

        [Fact]
        public void ProcessMessage_Should_Return_Null_For_Invalid_JSON()
        {
            // Arrange
            var message = "invalid json";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ProcessMessage_Should_Handle_Empty_Bids_And_Asks_Arrays()
        {
            // Arrange
            var message = @"{
                ""channel"": ""order_book_btcusd"",
                ""data"": {
                    ""bids"": [],
                    ""asks"": []
                }
            }";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("btcusd", result.Instrument);
            Assert.Empty(result.Bids);
            Assert.Empty(result.Asks);
        }

        [Fact]
        public void ProcessMessage_Should_Extract_Instrument_From_Channel()
        {
            // Arrange
            var message = @"{
                ""channel"": ""order_book_unknown_instrument"",
                ""data"": {
                    ""bids"": [],
                    ""asks"": []
                }
            }";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("instrument", result.Instrument);
        }

        [Fact]
        public void ProcessMessage_Should_Default_To_Unknown_When_No_Channel()
        {
            // Arrange
            var message = @"{
                ""data"": {
                    ""bids"": [],
                    ""asks"": []
                }
            }";

            // Act
            var result = _service.ProcessMessage(message);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("unknown", result.Instrument);
        }
    }
}
