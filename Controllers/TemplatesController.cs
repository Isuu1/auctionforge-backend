using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using SanityBackend.Services; // Make sure this namespace matches your SanityService location

namespace SanityBackend.Controllers
{

    [ApiController]
    [Route("api/[controller]")] // This means the endpoint will be /api/templates
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

        // Example for getting a single template by ID
        [HttpGet("{id}")] // This means the endpoint will be /api/templates/{id}
        public async Task<ActionResult<JsonDocument>> GetTemplateById(string id)
        {
            var template = await _sanityService.GetTemplateById(id);
            if (template == null)
            {
                return NotFound($"Template with ID '{id}' not found or an error occurred.");
            }
            return Ok(template);
        }
    }
}