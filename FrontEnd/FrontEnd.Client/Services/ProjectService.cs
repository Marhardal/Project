using FrontEnd.Client.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class ProjectService
    {

        private readonly HttpClient _http;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(HttpClient http, ILogger<ProjectService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<ProjectDTO>> GetProjectAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<ProjectDTO>>($"api/Projects/");
                return result ?? new List<ProjectDTO>();
            }
            catch (Exception ex)
            {
                // Log full exception including inner exceptions to help root-cause analysis
                _logger.LogError(ex, "Failed to GET proponents from {BaseAddress}{Endpoint}", _http.BaseAddress, "api/Proponents");
                if (ex is TaskCanceledException)
                {
                    _logger.LogWarning("Request was canceled - possible timeout or abort.");
                }
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {Inner}", ex.InnerException.Message);
                }

                // Return empty list to avoid bubbling exceptions to the UI lifecycle.
                return new List<ProjectDTO>();
            }
        }

        public async Task<HttpResponseMessage> CreateProjectAsync(ProjectDTO dto)
        {
            try
            {
                return await _http.PostAsJsonAsync("api/projects", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create project via POST {Endpoint}", "api/projects");
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }

    }
}
