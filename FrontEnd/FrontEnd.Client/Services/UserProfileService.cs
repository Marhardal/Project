using FrontEnd.Client.DTOs;
using System.Net;
using System.Net.Http.Json;

    public class UserProfileService
    {
        private readonly HttpClient _http;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(HttpClient http, ILogger<UserProfileService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(Guid userId)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<UserProfileDTO>($"api/Users/{userId}");
                return result ?? new UserProfileDTO();
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
                return new UserProfileDTO();
            }
        }

    public async Task<HttpResponseMessage> CreateProfileAsync(UserProfileDTO dto)
    {
        try
        {
            return await _http.PostAsJsonAsync("api/Users", dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Profile via POST {Endpoint}", "api/Profile");
            var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent(ex.Message)
            };
            return resp;
        }
    }


}

