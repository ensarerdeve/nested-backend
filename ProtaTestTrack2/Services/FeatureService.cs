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
                if (parentFeature != null)
                {
                    parentFeature.ChildFeatures.Add(feature);
                    await UpdateFeatureAndParentsAsync(parentFeature);

                    return feature;
                }
                else
                {
                    throw new InvalidOperationException($"Parent feature with ID {parentId} not found.");
                }
            }
            else
            {
                await _featureRepository.AddAsync(feature);
                return feature;
            }
        }

        private async Task UpdateFeatureAndParentsAsync(Feature feature)
        {
            await _featureRepository.UpdateAsync(feature);
            if (!string.IsNullOrEmpty(feature.ParentFeatureID))
            {
                var parentFeature = await _featureRepository.GetByIdAsync(Guid.Parse(feature.ParentFeatureID));
                if (parentFeature != null)
                {
                    await UpdateFeatureAndParentsAsync(parentFeature);
                }
            }
        }
        public async Task DeleteFeatureAsync(Guid id)
        {
            var feature = await _featureRepository.GetByIdAsync(id);
            if (feature != null)
            {
                await _featureRepository.DeleteAsync(id);
            }
            else
            {
                throw new InvalidOperationException($"Feature with ID {id} not found.");
            }
        }
        public async Task UpdateFeatureAsync(Feature updatedFeature)
        {
            var existingFeature = await _featureRepository.GetByIdAsync(updatedFeature.FeatureID);
            if (existingFeature == null)
            {
                throw new InvalidOperationException($"Feature with ID {updatedFeature.FeatureID} not found.");
            }

            existingFeature.Name = updatedFeature.Name;

            await _featureRepository.UpdateAsync(existingFeature);
        }
        public async Task AddChildFeatureAsync(Guid parentId, Feature childFeature)
        {
            var parentFeature = await _featureRepository.GetByIdAsync(parentId);
            if (parentFeature != null)
            {
                childFeature.FeatureID = Guid.NewGuid();
                parentFeature.ChildFeatures.Add(childFeature);

                await _featureRepository.UpdateAsync(parentFeature);
            }
            else
            {
                throw new InvalidOperationException($"Parent feature with ID {parentId} not found.");
            }
        }


    }
}
