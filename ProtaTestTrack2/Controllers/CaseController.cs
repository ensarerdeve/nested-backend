using Microsoft.AspNetCore.Mvc;
using ProtaTestTrack2.Services;

namespace ProtaTestTrack2.Controllers
{
    [ApiController]
    [Route("Case")]
    public class CaseController : ControllerBase
    {
        private readonly CaseService _caseService;

        public CaseController(CaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Case>>> GetAllCases()
        {
            try
            {
                var cases = await _caseService.GetAllCasesAsync();
                return Ok(cases);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Case>> GetCase(string id)
        {
            try
            {
                var caseItem = await _caseService.GetCaseByIdAsync(id);
                if (caseItem == null)
                {
                    return NotFound();
                }
                return Ok(caseItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Case>> CreateCase([FromBody] Case caseItem)
        {
            try
            {
                var createdCase = await _caseService.CreateCaseAsync(caseItem);
                return CreatedAtAction(nameof(GetCase), new { id = createdCase.CaseID }, createdCase);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCase(string id, [FromBody] Case caseItem)
        {
            try
            {
                if (id != caseItem.CaseID)
                {
                    return BadRequest("ID mismatch.");
                }
                await _caseService.UpdateCaseAsync(id, caseItem);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(string id)
        {
            try
            {
                await _caseService.DeleteCaseAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
