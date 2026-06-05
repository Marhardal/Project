using FrontEnd.Client.DTOs;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class CategoryService
    {
        private readonly HttpClient _http;
        private readonly ILogger<LocationService> _logger;

        public CategoryService(HttpClient http, ILogger<LocationService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<CategoryDTO>> GetLocationsAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<CategoryDTO>>($"api/Category");
                return result ?? new List<CategoryDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to GET Category from {BaseAddress}{Endpoint}", _http.BaseAddress, "api/Category");
                return new List<CategoryDTO>();
            }
        }

    }
}
