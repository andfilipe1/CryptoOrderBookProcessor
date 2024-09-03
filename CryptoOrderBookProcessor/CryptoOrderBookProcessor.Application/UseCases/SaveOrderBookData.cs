using CryptoOrderBookProcessor.Domain.Entities;
using CryptoOrderBookProcessor.Domain.Interfaces;

namespace CryptoOrderBookProcessor.Application.UseCases
{
    public class SaveOrderBookData(IOrderBookRepository orderBookRepository)
    {
        private readonly IOrderBookRepository _orderBookRepository = orderBookRepository;

        public async Task ExecuteAsync(OrderBook orderBook, CancellationToken cancellationToken)
        {
            await _orderBookRepository.SaveOrderBookAsync(orderBook, cancellationToken);
        }
    }
}
