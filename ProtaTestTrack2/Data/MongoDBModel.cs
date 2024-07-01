using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProtaTestTrack2.Data
{
    public class MongoDBModel
    {
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;
        
        public MongoDBModel(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
            _collectionName = settings.Value.CollectionName;
        }
        
        public IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(_collectionName);
        }
    }
}
