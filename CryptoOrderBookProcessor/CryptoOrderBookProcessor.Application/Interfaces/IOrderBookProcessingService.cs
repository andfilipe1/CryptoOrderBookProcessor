using CryptoOrderBookProcessor.Domain.Entities;

namespace CryptoOrderBookProcessor.Application.Interfaces
{
    public interface IOrderBookProcessingService
    {
        OrderBook? ProcessMessage(string message);
    }
}
