using FrontEnd.Client.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class ProponentService
    {
        private readonly HttpClient _http;
        private readonly ILogger<ProponentService> _logger;

        public ProponentService(HttpClient http, ILogger<ProponentService> logger)
        {
            _http = http;
            _logger = logger;
        }

        // Get all proponents
        public async Task<List<ProponentsDTO>> GetProponentsAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<ProponentsDTO>>("api/proponents");
                return result ?? new List<ProponentsDTO>();
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
                return new List<ProponentsDTO>();
            }
        }

        public async Task<ProponentsDTO> GetProponentAsync(Guid id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ProponentsDTO>($"api/proponents/{id}");
                return result ?? new ProponentsDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to GET proponent from {BaseAddress}{Endpoint}", _http.BaseAddress, $"api/proponents/{id}");
                return new ProponentsDTO();
            }
        }

        public async Task<HttpResponseMessage> CreateProponentAsync(ProponentsDTO dto)
        {
            try
            {
                return await _http.PostAsJsonAsync("api/proponents", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create proponent via POST {Endpoint}", "api/proponents");
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }

        // NEW: Update method for Edit
        public async Task<HttpResponseMessage> UpdateProponentAsync(Guid id, ProponentsDTO dto)
        {
            try
            {
                return await _http.PutAsJsonAsync($"api/proponents/{id}", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update proponent {Id} via PUT {Endpoint}", id, $"api/proponents/{id}");
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }
    }
}
