using Microsoft.AspNetCore.Mvc;
using ProtaTestTrack2.Services;

namespace ProtaTestTrack2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureController : ControllerBase
    {
        private readonly FeatureService _featureService;

        public FeatureController(FeatureService featureService)
        {
            _featureService = featureService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feature>>> GetAllFeatures()
        {
            try
            {
                var features = await _featureService.GetAllFeaturesAsync();
                return Ok(features);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Feature>> GetFeature(Guid id)
        {
            try
            {
                var feature = await _featureService.GetFeatureByIdAsync(id);
                if (feature == null)
                {
                    return NotFound();
                }
                return Ok(feature);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Feature>> CreateFeature([FromQuery] Guid? parentId, [FromBody] Feature feature)
        {
            try
            {
                var createdFeature = await _featureService.CreateFeatureAsync(parentId, feature);
                return CreatedAtAction(nameof(GetFeature), new { id = createdFeature.FeatureID }, createdFeature);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeature(Guid id)
        {
            try
            {
                await _featureService.DeleteFeatureAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeature(Guid id, [FromBody] Feature updatedFeature)
        {
            try
            {
                if (id != updatedFeature.FeatureID)
                {
                    return BadRequest("ID mismatch between route parameter and payload.");
                }

                await _featureService.UpdateFeatureAsync(updatedFeature);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost("{parentId}/child")]
        public async Task<ActionResult<Feature>> AddChildFeature(Guid parentId, [FromBody] Feature childFeature)
        {
            try
            {
                await _featureService.AddChildFeatureAsync(parentId, childFeature);
                return CreatedAtAction(nameof(GetFeature), new { id = childFeature.FeatureID }, childFeature);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
