using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OrderBookMicroservice.Domain.Interfaces;
using OrderBookMicroservice.Infrastructure.Repositories;

namespace OrderBookMicroservice.Infrastructure.Configurations
{
    public static class MongoDbConfiguration
    {
        public static void AddMongoDb(this IServiceCollection services, string connectionString, string databaseName)
        {
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase(databaseName);
            services.AddSingleton<IOrderBookRepository>(provider => new MongoOrderBookRepository(database));
        }
    }
}
