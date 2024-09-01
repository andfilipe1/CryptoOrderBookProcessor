using MongoDB.Driver;
using OrderBookMicroservice.Domain.Entities;
using OrderBookMicroservice.Domain.Interfaces;

namespace OrderBookMicroservice.Infrastructure.Repositories
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
