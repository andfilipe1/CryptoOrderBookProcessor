using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using CryptoOrderBookProcessor.Domain.Interfaces;
using CryptoOrderBookProcessor.Infrastructure.Repositories;

namespace CryptoOrderBookProcessor.Infrastructure.Configurations
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
