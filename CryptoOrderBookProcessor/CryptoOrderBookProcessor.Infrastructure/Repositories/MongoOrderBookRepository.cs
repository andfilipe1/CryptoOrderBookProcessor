using MongoDB.Driver;
using CryptoOrderBookProcessor.Domain.Entities;
using CryptoOrderBookProcessor.Domain.Interfaces;

namespace CryptoOrderBookProcessor.Infrastructure.Repositories
{
    public class MongoOrderBookRepository(IMongoDatabase database) : IOrderBookRepository
    {
        private readonly IMongoCollection<OrderBook> _orderBookCollection = database.GetCollection<OrderBook>("OrderBooks");
        public async Task SaveOrderBookAsync(OrderBook orderBook, CancellationToken cancellationToken)
        {
            await _orderBookCollection.InsertOneAsync(orderBook, null, cancellationToken);
        }
    }
}
