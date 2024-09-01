using OrderBookMicroservice.Application.Interfaces;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace OrderBookMicroservice.Application.Services
{
    public class WebSocketService(ILogger<WebSocketService> logger) : IWebSocketService
    {
        private readonly ClientWebSocket _webSocket = new();

        public async Task ConnectAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _webSocket.ConnectAsync(new Uri("wss://ws.bitstamp.net"), stoppingToken);
                logger.LogInformation("Conectado ao WebSocket.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao conectar ao WebSocket.");
                throw;
            }
        }

        public async Task SubscribeToOrderBook(string instrument, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"Assinando o livro de ordens de {instrument}...");
                var subscriptionMessage = new { @event = "bts:subscribe", data = new { channel = $"order_book_{instrument}" } };
                var message = JsonSerializer.Serialize(subscriptionMessage);
                var segment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, cancellationToken);
                logger.LogInformation($"Assinado com sucesso ao livro de ordens de {instrument}.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao assinar o livro de ordens de {instrument}.");
                throw;
            }
        }

        public async Task<string> ReceiveMessageAsync(CancellationToken stoppingToken)
        {
            var buffer = new byte[1024 * 16];
            WebSocketReceiveResult result;
            var receivedMessage = new List<byte>();

            try
            {
                do
                {
                    result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), stoppingToken);
                    receivedMessage.AddRange(buffer.Take(result.Count));
                } while (!result.EndOfMessage);

                var message = Encoding.UTF8.GetString(receivedMessage.ToArray());
                logger.LogInformation($"Mensagem recebida: {message}");
                return message;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao receber mensagem do WebSocket.");
            }

            return string.Empty;
        }

        public void Dispose()
        {
            _webSocket.Dispose();
        }
    }
}