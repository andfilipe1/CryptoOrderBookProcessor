using OrderBookMicroservice.Application.Interfaces;
using OrderBookMicroservice.Domain.Entities;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace OrderBookMicroservice.Application.Services
{
    public class OrderBookProcessingService(ILogger<OrderBookProcessingService> logger) : IOrderBookProcessingService
    {
        private readonly ILogger<OrderBookProcessingService> _logger = logger;

        public OrderBook? ProcessMessage(string message)
        {
            try
            {
                _logger.LogInformation("Processando mensagem recebida...");

                var jsonDocument = JsonDocument.Parse(message);

                if (jsonDocument.RootElement.TryGetProperty("event", out var eventElement))
                {
                    var eventType = eventElement.GetString();
                    _logger.LogInformation($"Tipo de evento recebido: {eventType}");

                    if (eventType == "bts:subscription_succeeded")
                    {
                        _logger.LogInformation("Mensagem de confirmação de subscrição recebida. Ignorando...");
                        return null;
                    }
                }

                if (!jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
                {
                    _logger.LogWarning("A chave 'data' não foi encontrada na mensagem JSON.");
                    return null;
                }

                var instrument = jsonDocument.RootElement.GetProperty("channel").GetString()?.Split('_').LastOrDefault() ?? "unknown";
                _logger.LogInformation($"Processando dados para o instrumento: {instrument}");

                var orderBook = new OrderBook { Instrument = instrument };

                if (dataElement.TryGetProperty("bids", out var bidsElement))
                {
                    _logger.LogInformation($"Encontrados {bidsElement.GetArrayLength()} bids.");
                    foreach (var bid in bidsElement.EnumerateArray())
                    {
                        decimal price = decimal.Parse(bid[0].GetString() ?? "0");
                        decimal quantity = decimal.Parse(bid[1].GetString() ?? "0");
                        orderBook.Bids.Add(new Order { Price = price, Quantity = quantity });
                        _logger.LogInformation($"Adicionando Bid - Preço: {price}, Quantidade: {quantity}");
                    }
                }
                else
                {
                    _logger.LogWarning("Elemento 'bids' não encontrado no JSON.");
                }

                if (dataElement.TryGetProperty("asks", out var asksElement))
                {
                    _logger.LogInformation($"Encontrados {asksElement.GetArrayLength()} asks.");
                    foreach (var ask in asksElement.EnumerateArray())
                    {
                        decimal price = decimal.Parse(ask[0].GetString() ?? "0");
                        decimal quantity = decimal.Parse(ask[1].GetString() ?? "0");
                        orderBook.Asks.Add(new Order { Price = price, Quantity = quantity });
                        _logger.LogInformation($"Adicionando Ask - Preço: {price}, Quantidade: {quantity}");
                    }
                }
                else
                {
                    _logger.LogWarning("Elemento 'asks' não encontrado no JSON.");
                }

                _logger.LogInformation("Mensagem processada com sucesso.");
                return orderBook;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a mensagem.");
                return null;
            }
        }
    }
}
