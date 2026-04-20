using FrontEnd.DTOs;

namespace FrontEnd.Services
{
    public class StatusService
    {
        private readonly HttpClient _http;
        private readonly ILogger<StatusDTO> _logger;

        public StatusService(HttpClient http, ILogger<StatusDTO> logger)
        {
            _http = http;
            _logger = logger;
        }

        // Get all proponents
        public async Task<List<StatusDTO>> GetStatusAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<StatusDTO>>("api/Status");
                return result ?? new List<StatusDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to GET proponents from {BaseAddress}{Endpoint}", _http.BaseAddress, "api/proponents");
                if (ex is TaskCanceledException)
                {
                    _logger.LogWarning("Request was canceled - possible timeout or abort.");
                }
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {Inner}", ex.InnerException.Message);
                }
                return new List<StatusDTO>();
            }
        }

    }
}
