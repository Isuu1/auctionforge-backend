using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using SanityBackend.Services;

namespace SanityBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This means the endpoint will be /api/colorpalette
    public class ColorPaletteController : ControllerBase
    {
        private readonly SanityService _sanityService;

        public ColorPaletteController(SanityService sanityService)
        {
            _sanityService = sanityService;
        }

        [HttpGet]
        public async Task<ActionResult<JsonDocument>> GetColorPalette()
        {
            var colorPalette = await _sanityService.GetColorPalette();
            if (colorPalette == null)
            {
                return StatusCode(500, "Failed to fetch color palette from Sanity.");
            }
            return Ok(colorPalette);
        }
    }
}