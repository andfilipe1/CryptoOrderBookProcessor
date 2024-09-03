using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CryptoOrderBookProcessor.Application.Interfaces;
using CryptoOrderBookProcessor.Application.UseCases;

namespace CryptoOrderBookProcessor.Worker
{
    public class Worker(SaveOrderBookData saveOrderBookDataUseCase, IWebSocketService webSocketService, IOrderBookProcessingService orderBookProcessingService, IMetricsService metricsService, ILogger<Worker> logger) : BackgroundService
    {
        private readonly SaveOrderBookData _saveOrderBookDataUseCase = saveOrderBookDataUseCase;
        private readonly IWebSocketService _webSocketService = webSocketService;
        private readonly IOrderBookProcessingService _orderBookProcessingService = orderBookProcessingService;
        private readonly IMetricsService _metricsService = metricsService;
        private readonly ILogger<Worker> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Tentando conectar ao WebSocket...");
                await _webSocketService.ConnectAsync(stoppingToken);
                _logger.LogInformation("Conectado ao WebSocket.");

                await _webSocketService.SubscribeToOrderBook("btcusd", stoppingToken);
                await _webSocketService.SubscribeToOrderBook("ethusd", stoppingToken);

                var metricsTask = _metricsService.CalculateMetricsEvery5Seconds(stoppingToken);
                await ReceiveMessages(stoppingToken);
                await metricsTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar o Worker.");
            }
        }

        private async Task ReceiveMessages(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Aguardando mensagens...");
                var message = await _webSocketService.ReceiveMessageAsync(stoppingToken);

                if (!string.IsNullOrEmpty(message))
                {
                    var orderBook = _orderBookProcessingService.ProcessMessage(message);
                    if (orderBook != null)
                    {
                        await _saveOrderBookDataUseCase.ExecuteAsync(orderBook, stoppingToken);
                        _metricsService.ProcessOrderBookData(orderBook);
                    }
                }
            }
        }

        public override void Dispose()
        {
            _webSocketService.Dispose();
            base.Dispose();
        }
    }
}
