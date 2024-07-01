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

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await FindByIdRecursiveAsync(id);
        }

        public async Task<T> FindByIdRecursiveAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Or(
                Builders<T>.Filter.Eq("RootID", id),
                Builders<T>.Filter.Eq("FeatureID", id),
                Builders<T>.Filter.Eq("CaseID", id)
            );

            var entity = await _collection.Find(filter).FirstOrDefaultAsync();
            if (entity != null)
            {
                return entity;
            }

            var allEntities = await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
            foreach (var parentEntity in allEntities)
            {
                var childFeatures = GetChildFeatures(parentEntity);
                if (childFeatures != null)
                {
                    foreach (var childFeature in childFeatures)
                    {
                        if (GetEntityId(childFeature) == id)
                        {
                            return childFeature;
                        }
                    }
                }
            }

            return null;
        }

        private List<T> GetChildFeatures(T parentEntity)
        {
            var propertyInfo = parentEntity.GetType().GetProperty("ChildFeatures");
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(parentEntity) as List<T>;
            }
            return null;
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

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Or(
                Builders<T>.Filter.Eq("RootID", id),
                Builders<T>.Filter.Eq("FeatureID", id),
                Builders<T>.Filter.Eq("CaseID", id)
            );

            await _collection.DeleteOneAsync(filter);
        }

        private Guid GetEntityId(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("Id")
                                ?? entity.GetType().GetProperty("FeatureID")
                                ?? entity.GetType().GetProperty("RootID")
                                ?? entity.GetType().GetProperty("CaseID");

            if (propertyInfo == null)
            {
                throw new ArgumentException("Entity does not have an Id.");
            }

            return (Guid)propertyInfo.GetValue(entity);
        }
    }
}
