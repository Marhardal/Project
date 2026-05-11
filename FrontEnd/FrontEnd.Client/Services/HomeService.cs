using FrontEnd.Client.DTOs;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class HomeService
    {
        private readonly HttpClient _http;
        private readonly ILogger<HomeService> _logger;

        public HomeService(HttpClient http, ILogger<HomeService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<StatusSummaryDTO>> GetStatusSummary(Guid ProponentID)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<StatusSummaryDTO>>($"api/status-summary");
                return result ?? new List<StatusSummaryDTO>();
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
                return new List<StatusSummaryDTO>();
            }
        }

        public async Task<List<ProjectbyProponentDTO>> GetProjectbyProponent()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<ProjectbyProponentDTO>>($"api/GetProjectbyProponent");
                return result ?? new List<ProjectbyProponentDTO>();
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
                return new List<ProjectbyProponentDTO>();
            }
        }

        public async Task<List<RecentProjectsDTO>> GetRecentProjects()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<RecentProjectsDTO>>($"api/GetRecentProjects");
                return result ?? new List<RecentProjectsDTO>();
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
                return new List<RecentProjectsDTO>();
            }
        }

        public async Task<List<GroupedProjectTypesDTO>> GetGroupedProjects()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<GroupedProjectTypesDTO>>($"api/GetGroupedProjects");
                return result ?? new List<GroupedProjectTypesDTO>();
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
                return new List<GroupedProjectTypesDTO>();
            }
        }

        public async Task<List<ProjectStatusDTO>> GetProjectStatuses()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<ProjectStatusDTO>>($"api/GetProjectStatus");
                return result ?? new List<ProjectStatusDTO>();
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
                return new List<ProjectStatusDTO>();
            }
        }

    }
}
