namespace CryptoOrderBookProcessor.Application.Interfaces
{
    public interface IWebSocketService : IDisposable
    {
        Task ConnectAsync(CancellationToken stoppingToken);
        Task SubscribeToOrderBook(string instrument, CancellationToken cancellationToken);
        Task<string> ReceiveMessageAsync(CancellationToken stoppingToken);
    }
}
