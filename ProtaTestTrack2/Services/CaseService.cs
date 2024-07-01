using ProtaTestTrack2.Repository;

namespace ProtaTestTrack2.Services
{
    public class CaseService
    {
        private readonly IRepository<Case> _caseRepository;
        private readonly IRepository<Feature> _featureRepository;

        public CaseService(IRepository<Case> caseRepository, IRepository<Feature> featureRepository)
        {
            _caseRepository = caseRepository;
            _featureRepository = featureRepository;
        }

        public async Task<IEnumerable<Case>> GetAllCasesAsync()
        {
            var cases = await _caseRepository.GetAllAsync();
            return cases;
        }

        public async Task<Case> GetCaseByIdAsync(Guid id)
        {
            var caseById = await _caseRepository.GetByIdAsync(id);
            return caseById;
        }

        public async Task<Case> CreateCaseAsync(Guid? featureId, Case caseItem)
        {
            if (featureId.HasValue)
            {
                var parentFeature = await _featureRepository.GetByIdAsync(featureId.Value);
                if (parentFeature == null)
                {
                    throw new Exception($"Feature with ID '{featureId}' not found.");
                }
                caseItem.CaseID = Guid.NewGuid();
                caseItem.ParentFeatureID = featureId.Value.ToString();
                parentFeature.Cases.Add(caseItem);
                await _featureRepository.UpdateAsync(parentFeature);
                return caseItem;
            }
            else
            {
                caseItem.CaseID = Guid.NewGuid();
                await _caseRepository.AddAsync(caseItem);
                return caseItem;
            }
        }

        public async Task UpdateCaseAsync(Guid id, Case caseItem)
        {
            caseItem.CaseID = id;
            await _caseRepository.UpdateAsync(caseItem);
        }

        public async Task DeleteCaseAsync(Guid id)
        {
            await _caseRepository.DeleteAsync(id);
        }
        public async Task AddCaseToChildFeatureAsync(Guid featureId, Guid childFeatureId, Case caseItem)
        {
            var parentFeature = await _featureRepository.GetByIdAsync(featureId);
            if (parentFeature == null)
            {
                throw new Exception($"Parent feature with ID '{featureId}' not found.");
            }

            var childFeature = parentFeature.ChildFeatures.FirstOrDefault(cf => cf.FeatureID == childFeatureId);
            if (childFeature == null)
            {
                throw new Exception($"Child feature with ID '{childFeatureId}' not found.");
            }

            caseItem.CaseID = Guid.NewGuid();
            caseItem.ParentFeatureID = childFeatureId.ToString();
            childFeature.Cases.Add(caseItem);
            await _featureRepository.UpdateAsync(parentFeature);
        }
    }
}
