using MongoDB.Driver;
using ProtaTestTrack2.Repository;

namespace ProtaTestTrack2.Services
{
    public class FeatureService
    {
        private readonly IRepository<Feature> _featureRepository;
        public FeatureService(IRepository<Feature> featureRepository)
        {
            _featureRepository = featureRepository;
        }
        public async Task<IEnumerable<Feature>> GetAllFeaturesAsync()
        {
            var features = await _featureRepository.GetAllAsync();
            return features;
        }
        public async Task<Feature> GetFeatureByIdAsync(Guid id)
        {
            var feature = await _featureRepository.GetByIdAsync(id);
            return feature;
        }
        public async Task<Feature> CreateFeatureAsync(Guid? parentId, Feature feature)
        {
            feature.FeatureID = Guid.NewGuid();
            if (parentId.HasValue)
            {
                var parentFeature = await _featureRepository.GetByIdAsync(parentId.Value);
                if (parentFeature == null)
                {
                    throw new Exception($"Parent feature with ID '{parentId}' not found.");
                }
                feature.ParentFeatureID = parentId.Value.ToString();
                parentFeature.ChildFeatures.Add(feature);
                await _featureRepository.UpdateAsync(parentFeature);
            }
            else
            {
                await _featureRepository.AddAsync(feature);
            }
            return feature;
        }
        public async Task DeleteFeatureAsync(Guid id)
        {
            await _featureRepository.DeleteAsync(id);
        }
        public async Task UpdateFeatureAsync(Feature updatedFeature)
        {
            await _featureRepository.UpdateAsync(updatedFeature);
        }
    }
}
