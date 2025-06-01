using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using SanityBackend.Services; 

namespace SanityBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //Endpoint: /api/templates
    public class TemplatesController : ControllerBase
    {
        private readonly SanityService _sanityService;

        public TemplatesController(SanityService sanityService)
        {
            _sanityService = sanityService;
        }

        [HttpGet]
        public async Task<ActionResult<JsonDocument>> GetTemplates()
        {
            var templates = await _sanityService.GetTemplates();
            if (templates == null)
            {
                return StatusCode(500, "Failed to fetch templates from Sanity.");
            }
            return Ok(templates);
        }

        [HttpGet("{slug}")] //Endpoint /api/templates/{slug}
        public async Task<ActionResult<JsonDocument>> GetTemplateBySlug(string slug)
        {
            var template = await _sanityService.GetTemplateBySlug(slug);
            if (template == null)
            {
                return NotFound($"Template with ID '{slug}' not found or an error occurred.");
            }
            return Ok(template);
        }
    }
}