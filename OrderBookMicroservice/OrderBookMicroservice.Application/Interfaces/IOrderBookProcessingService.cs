using OrderBookMicroservice.Domain.Entities;

namespace OrderBookMicroservice.Application.Interfaces
{
    public interface IOrderBookProcessingService
    {
        OrderBook? ProcessMessage(string message);
    }
}
