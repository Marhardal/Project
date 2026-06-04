using FrontEnd.Client.DTOs;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class LocationService
    {
        private readonly HttpClient _http;
        private readonly ILogger<LocationService> _logger;

        public LocationService(HttpClient http, ILogger<LocationService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<LocationDTO>> GetLocationsAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<LocationDTO>>($"api/Locations");
                return result ?? new List<LocationDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to GET Locations from {BaseAddress}{Endpoint}", _http.BaseAddress, "api/Locations");
                return new List<LocationDTO>();
            }
        }

    }
}
