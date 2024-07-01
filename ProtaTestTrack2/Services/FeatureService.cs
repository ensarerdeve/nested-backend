using MongoDB.Driver;
using ProtaTestTrack2.Repository;

namespace ProtaTestTrack2.Services
{
    public class FeatureService
    {
        private readonly IRepository<Feature> _featureRepository;
        private readonly IRepository<RootFeature> _rootFeatureRepository;

        public FeatureService(IRepository<Feature> featureRepository, IRepository<RootFeature> rootFeatureRepository)
        {
            _featureRepository = featureRepository;
            _rootFeatureRepository = rootFeatureRepository;
        }

        public async Task<IEnumerable<Feature>> GetAllFeaturesAsync()
        {
            var features = await _featureRepository.GetAllAsync();
            return features;
        }

        public async Task<Feature> GetFeatureByIdAsync(string id)
        {
            var feature = await _featureRepository.GetByIdAsync(id);
            return feature;
        }

        public async Task<Feature> CreateFeatureAsync(Feature feature)
        {
            feature.FeatureID = Guid.NewGuid().ToString();
            await _featureRepository.AddAsync(feature);

            var rootFeature = await _rootFeatureRepository.GetByIdAsync("RootFeatureID");
            if (rootFeature == null)
            {
                rootFeature = new RootFeature
                {
                    RootID = "RootFeatureID",
                    Features = new List<Feature>
                    {
                        new Feature
                        {
                            FeatureID = feature.FeatureID,
                            Name = feature.Name,
                            ParentFeatureID = feature.ParentFeatureID
                        }
                    }
                };
                await _rootFeatureRepository.AddAsync(rootFeature);
            }
            else
            {
                rootFeature.Features.Add(new Feature
                {
                    FeatureID = feature.FeatureID,
                    Name = feature.Name,
                    ParentFeatureID = feature.ParentFeatureID
                });
                await _rootFeatureRepository.UpdateAsync(rootFeature);
            }
            if (!string.IsNullOrEmpty(feature.ParentFeatureID))
            {
                var parentFeature = await _featureRepository.GetByIdAsync(feature.ParentFeatureID);
                if (parentFeature == null)
                {
                    throw new Exception($"Parent feature with ID '{feature.ParentFeatureID}' not found.");
                }
                parentFeature.ChildFeatures.Add(new Feature
                {
                    FeatureID = feature.FeatureID,
                    Name = feature.Name,
                    ParentFeatureID = feature.ParentFeatureID
                });
                await _featureRepository.UpdateAsync(parentFeature);
            }

            return feature;
        }
        public async Task DeleteFeatureAsync(string id)
        {
            await _featureRepository.DeleteAsync(id);

            var rootFeature = await _rootFeatureRepository.GetByIdAsync("RootFeatureID");
            if (rootFeature != null)
            {
                var featureToRemove = rootFeature.Features.FirstOrDefault(f => f.FeatureID == id);
                if (featureToRemove != null)
                {
                    rootFeature.Features.Remove(featureToRemove);
                    await _rootFeatureRepository.UpdateAsync(rootFeature);
                }
            }
        }

        public async Task UpdateFeatureAsync(Feature updatedFeature)
        {
            await _featureRepository.UpdateAsync(updatedFeature);
            var rootFeature = await _rootFeatureRepository.GetByIdAsync("RootFeatureID");
            if (rootFeature != null)
            {
                var featureToUpdate = rootFeature.Features.FirstOrDefault(f => f.FeatureID == updatedFeature.FeatureID);
                if (featureToUpdate != null)
                {
                    featureToUpdate.Name = updatedFeature.Name;
                    featureToUpdate.ParentFeatureID = updatedFeature.ParentFeatureID;
                    await _rootFeatureRepository.UpdateAsync(rootFeature);
                }
            }
            if (!string.IsNullOrEmpty(updatedFeature.ParentFeatureID))
            {
                var parentFeature = await _featureRepository.GetByIdAsync(updatedFeature.ParentFeatureID);
                if (parentFeature != null)
                {
                    var childToUpdate = parentFeature.ChildFeatures.FirstOrDefault(cf => cf.FeatureID == updatedFeature.FeatureID);
                    if (childToUpdate != null)
                    {
                        childToUpdate.Name = updatedFeature.Name;
                        childToUpdate.ParentFeatureID = updatedFeature.ParentFeatureID;
                        await _featureRepository.UpdateAsync(parentFeature);
                    }
                }
            }
        }
    }
}
