using Microsoft.AspNetCore.Mvc;
using ProtaTestTrack2.Repository;

namespace ProtaTestTrack2.Controllers
{
    [Route("Root")]
    [ApiController]
    public class RootFeatureController : ControllerBase
    {
        private readonly IRepository<RootFeature> _repository;

        public RootFeatureController(IRepository<RootFeature> repository)
        {
            _repository = repository;
        }
        // GET: api/rootfeature
        [HttpGet("GetAllRootFeatures")]
        public async Task<ActionResult<IEnumerable<RootFeature>>> GetAllRootFeatures()
        {
            try
            {
                var rootFeatures = await _repository.GetAllAsync();
                return Ok(rootFeatures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        // GET: api/rootfeature/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RootFeature>> GetRootFeature(Guid id)
        {
            try
            {
                var rootFeature = await _repository.GetByIdAsync(id);
                if (rootFeature == null)
                {
                    return NotFound();
                }
                return Ok(rootFeature);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        // POST: api/rootfeature
        [HttpPost]
        public async Task<ActionResult<RootFeature>> CreateRootFeature(RootFeature rootFeature)
        {
            try
            {
                rootFeature.RootID = Guid.NewGuid();
                await _repository.AddAsync(rootFeature);
                return CreatedAtAction(nameof(GetRootFeature), new { id = rootFeature.RootID }, rootFeature);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        // PUT: api/rootfeature/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRootFeature(Guid id, RootFeature rootFeature)
        {
            try
            {
                if (id != rootFeature.RootID)
                {
                    return BadRequest("Something wrong with ID.");
                }

                await _repository.UpdateAsync(rootFeature);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        // DELETE: api/rootfeature/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRootFeature(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}