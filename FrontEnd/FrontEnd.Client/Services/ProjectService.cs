using FrontEnd.Client.DTOs;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;


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

        public async Task<List<ProjectDTO>> GetProjectAsync(bool isProposal)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<ProjectDTO>>($"api/Projects/filter/{isProposal}");
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

        public async Task<ProjectDTO> GetProjectAsync(Guid id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ProjectDTO>($"api/projects/{id}");
                return result ?? new ProjectDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to GET project from {BaseAddress}{Endpoint}", _http.BaseAddress, $"api/projects/{id}");
                return new ProjectDTO();
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

        public async Task<HttpResponseMessage> UpdateProjectAsync(Guid id, ProjectDTO dto)
        {
            try
            {
                return await _http.PutAsJsonAsync($"api/projects/{id}", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update project {Id} via PUT {Endpoint}", id, $"api/projects/{id}");
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }
        public async Task<byte[]> ExportProjectAsync(string? filter = null, bool isProposal = true)
        {
            var url = $"api/export/projects/excel?filter={filter}&Proposal={isProposal}";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return Array.Empty<byte>();

            return await response.Content.ReadAsByteArrayAsync();
        }
        public async Task<byte[]> ExportProjectPDFAsync(string? filter = null, bool isProposal = true)
        {
            var url = $"api/export/projects/pdf?filter={filter}&Proposal={isProposal}";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return Array.Empty<byte>();

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
