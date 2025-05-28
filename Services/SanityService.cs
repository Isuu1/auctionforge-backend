using System.Net.Http;
using System.Text.Json; // For System.Text.Json
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // To access appsettings.json

namespace SanityBackend.Services
{
    public class SanityService
    {
        private readonly HttpClient _httpClient;
        private readonly string _projectId;
        private readonly string _dataset;
        private readonly string _apiVersion;

        public SanityService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _projectId = configuration["Sanity:ProjectId"];
            _dataset = configuration["Sanity:Dataset"];
            _apiVersion = configuration["Sanity:ApiVersion"];

            if (string.IsNullOrEmpty(_projectId) || string.IsNullOrEmpty(_dataset) || string.IsNullOrEmpty(_apiVersion))
            {
                throw new ArgumentNullException("Sanity configuration (ProjectId, Dataset, ApiVersion) is missing in appsettings.json.");
            }

            // Base URL for Sanity's content API
            _httpClient.BaseAddress = new Uri($"https://{_projectId}.api.sanity.io/{_apiVersion}/data/query/{_dataset}/");
        }

        // Example method to fetch all documents of type 'template'
        public async Task<JsonDocument?> GetTemplates()
        {
            // GROQ query: *[_type == 'template']
            var query = "*[_type == 'template']";
            var encodedQuery = System.Web.HttpUtility.UrlEncode(query); // Encode the query
            var requestUri = $"?query={encodedQuery}";

            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonDocument.Parse(content);
            }
            else
            {
                // Log the error or throw an exception
                Console.WriteLine($"Error fetching templates from Sanity: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return null;
            }
        }

        public async Task<JsonDocument> GetColorPalette()
        {
            var query = "*[_type == 'colorPalette']";
            var encodedQuery = System.Web.HttpUtility.UrlEncode(query);
            var requestUri = $"?query={encodedQuery}";

            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonDocument.Parse(content);
            }
            else
            {
                Console.WriteLine($"Error fetching color palette from Sanity: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return null;
            }
        }

        // You can add more methods here for other queries (e.g., get a specific template by ID, create a listing, etc.)
        // Example for fetching a single template by ID:
        public async Task<JsonDocument> GetTemplateBySlug(string slug)
        {
            var query = $"*[slug.current == '{slug}']";
            var encodedQuery = System.Web.HttpUtility.UrlEncode(query);
            var requestUri = $"?query={encodedQuery}";

            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonDocument.Parse(content);
            }
            else
            {
                Console.WriteLine($"Error fetching template by ID '{slug}': {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return null;
            }
        }
    }
}