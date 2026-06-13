using FrontEnd.Client.DTOs;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class PermissionService
    {
        private readonly HttpClient _http;
        private List<PermissionDto> _permissions = [];
        private bool _initialized;

        public PermissionService(HttpClient http) => _http = http;

        public async Task InitializeAsync()
        {
            if (_initialized) return;
            try
            {
                _permissions = await _http.GetFromJsonAsync<List<PermissionDto>>("api/permissions/my-nav") ?? [];
            }
            catch
            {
                _permissions = [];
            }
            _initialized = true;
        }

        public void Reset()
        {
            _permissions.Clear();
            _initialized = false;
        }

        // Can the user see this page at all?
        public bool CanView(string pageSlug) =>
            _permissions.Any(p => p.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase)
                && p.ActionName.Equals("View", StringComparison.OrdinalIgnoreCase));

        // Can the user perform a specific action on this page?
        public bool Can(string pageSlug, string actionName) =>
            _permissions.Any(p => p.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase)
                && p.ActionName.Equals(actionName, StringComparison.OrdinalIgnoreCase));

        public List<PermissionDto> AllPermissions => _permissions;
    }
}
