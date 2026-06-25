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

        public async Task<PagedResult<ProjectDTO>> GetProjectAsync(bool isProposal, int Page = 1, int pageSize = 10)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<PagedResult<ProjectDTO>>($"api/Projects/filter/{isProposal}?Page={Page}&pageSize={pageSize}");
                return result ?? new PagedResult<ProjectDTO>();
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
                return new PagedResult<ProjectDTO>();
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
        public async Task<byte[]> ExportProjectAsync(ProjectType? type = null, string? filter = null, string? statusID = null)
        {
            var url = $"api/export/projects/excel?filter={filter}&statusID={statusID}&type={type}";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return Array.Empty<byte>();

            return await response.Content.ReadAsByteArrayAsync();
        }
        public async Task<byte[]> ExportProjectPDFAsync(ProjectType? type = null, string? filter = null, string? statusID = null)
        {
            var url = $"api/export/projects/pdf?filter={filter}&statusID={statusID}&type={type}";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return Array.Empty<byte>();

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
