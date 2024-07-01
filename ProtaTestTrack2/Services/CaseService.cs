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

        public async Task<Case> GetCaseByIdAsync(string id)
        {
            var caseById = await _caseRepository.GetByIdAsync(id);
            return caseById;
        }

        public async Task<Case> CreateCaseAsync(Case caseItem)
        {
            if (caseItem.ParentFeatureID != null)
            {
                var parentFeature = await _featureRepository.GetByIdAsync(caseItem.ParentFeatureID);
                if (parentFeature == null)
                {
                    throw new Exception($"Feature with ID '{caseItem.ParentFeatureID}' not found.");
                }
                caseItem.CaseID = Guid.NewGuid().ToString();
                parentFeature.Cases.Add(caseItem);
                await _featureRepository.UpdateAsync(parentFeature);
                return caseItem;
            }
            else
            {
                caseItem.CaseID = Guid.NewGuid().ToString();;
                await _caseRepository.AddAsync(caseItem);
                return caseItem;
            }
        }

        public async Task UpdateCaseAsync(string id, Case caseItem)
        {
            caseItem.CaseID = id;
            await _caseRepository.UpdateAsync(caseItem);
        }

        public async Task DeleteCaseAsync(string id)
        {
            await _caseRepository.DeleteAsync(id);
        }
    }
}
