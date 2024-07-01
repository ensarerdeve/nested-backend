using MongoDB.Driver;
using ProtaTestTrack2.Data;

namespace ProtaTestTrack2.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;
        public Repository(MongoDBModel database)
        {
            _collection = database.GetCollection<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Or(
                Builders<T>.Filter.Eq("RootID", id),
                Builders<T>.Filter.Eq("FeatureID", id),
                Builders<T>.Filter.Eq("CaseID", id)
            );
            var result = await _collection.FindAsync<T>(filter);
            return await result.FirstOrDefaultAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            var id = GetEntityId(entity);
            var filter = Builders<T>.Filter.Or(
                Builders<T>.Filter.Eq("RootID", id),
                Builders<T>.Filter.Eq("FeatureID", id),
                Builders<T>.Filter.Eq("CaseID", id)
            );

            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Or(
                Builders<T>.Filter.Eq("RootID", id),
                Builders<T>.Filter.Eq("FeatureID", id),
                Builders<T>.Filter.Eq("CaseID", id)
            );

            await _collection.DeleteOneAsync(filter);
        }

        private string GetEntityId(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("Id")
                                ?? entity.GetType().GetProperty("FeatureID")
                                ?? entity.GetType().GetProperty("RootID")
                                ?? entity.GetType().GetProperty("CaseID");

            if (propertyInfo == null)
            {
                throw new ArgumentException("Entity does not have an Id.");
            }

            return (string)propertyInfo.GetValue(entity);
        }
    }
}
