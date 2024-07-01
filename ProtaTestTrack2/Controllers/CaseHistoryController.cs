using Microsoft.AspNetCore.Mvc;
using ProtaTestTrack2.Repository;

namespace ProtaTestTrack2.Controllers
{
    [Route("CaseHistory")]
    [ApiController]
    public class CaseHistoryController : ControllerBase
    {
        private readonly IRepository<CaseHistory> _caseHistoryRepository;
        public CaseHistoryController(IRepository<CaseHistory> caseHistoryRepository)
        {
            _caseHistoryRepository = caseHistoryRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseHistory>>> GetCaseHistory()
        {
            var caseHistories = await _caseHistoryRepository.GetAllAsync();
            return Ok(caseHistories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CaseHistory>> GetCaseHistory(Guid id)
        {
            var caseHistory = await _caseHistoryRepository.GetByIdAsync(id);
            if (caseHistory == null)
            {
                return NotFound();
            }
            return Ok(caseHistory);
        }
        [HttpPost]
        public async Task<ActionResult<CaseHistory>> PostCaseHistory(CaseHistory caseHistory)
        {
            try
            {
                await _caseHistoryRepository.AddAsync(caseHistory);
                return CreatedAtAction(nameof(GetCaseHistory), new { id = caseHistory.ParentCaseID }, caseHistory);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaseHistory(string id, CaseHistory caseHistory)
        {
            if (id != caseHistory.ParentCaseID)
            {
                return BadRequest();
            }
            try
            {
                await _caseHistoryRepository.UpdateAsync(caseHistory);
                return Ok(caseHistory);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCaseHistory(Guid id)
        {
            try
            {
                await _caseHistoryRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
