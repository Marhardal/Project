using FrontEnd.Client.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class PermissionManagementService
    {

        private readonly HttpClient _http;
        private readonly ILogger<PermissionManagementService> _logger;

        public PermissionManagementService(HttpClient http, ILogger<PermissionManagementService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<RolesDTO>> GetRolesAsync()
        {
            var result = await _http.GetFromJsonAsync<List<RolesDTO>>("api/roles");
            return result ?? [];
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var response = await _http.PostAsJsonAsync("api/roles", roleName);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var response = await _http.DeleteAsync($"api/roles/{roleId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<PagesDTO>> GetPagesAsync()
        {
            var result = await _http.GetFromJsonAsync<List<PagesDTO>>("api/pages");
            return result ?? [];
        }

        public async Task<HttpResponseMessage?> CreatePageAsync(PagesDTO dto)
        {

            try
            {
                return await _http.PostAsJsonAsync("api/pages", dto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to add a Page and its actions via POST {Endpoint}", "api/pages");
                // Return a synthetic failure response so callers can handle gracefully
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }

        public async Task<HttpResponseMessage?> updatePageAsync(Guid Id, PagesDTO dto)
        {

            try
            {
                return await _http.PutAsJsonAsync("api/pages/"+Id, dto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to add a Page and its actions via POST {Endpoint}", "api/pages");
                // Return a synthetic failure response so callers can handle gracefully
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }

        public async Task<bool> DeletePageAsync(Guid pageId)
        {
            var response = await _http.DeleteAsync($"api/pages/{pageId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Guid?> AddActionAsync(Guid pageId, string actionName)
        {
            var response = await _http.PostAsJsonAsync($"api/pages/{pageId}/actions", actionName);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task<bool> DeleteActionAsync(Guid actionId)
        {
            var response = await _http.DeleteAsync($"api/pages/actions/{actionId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SavePermissionsAsync(string roleId, List<SavePermissionDto> permissions)
        {
            var response = await _http.PutAsJsonAsync($"api/roles/{roleId}/permissions", permissions);
            return response.IsSuccessStatusCode;
        }
    }
}
