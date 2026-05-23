using FrontEnd.Client;
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

    public async Task<List<UserProfileDTO>> GetUserProfilesAsync()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<UserProfileDTO>>($"api/Users");
            return result ?? new List<UserProfileDTO>();
        }
        catch (Exception ex)
        {
            // Log full exception including inner exceptions to help root-cause analysis
            _logger.LogError(ex, "Failed to GET user profiles from {BaseAddress}{Endpoint}", _http.BaseAddress, "api/Users");
            if (ex is TaskCanceledException)
            {
                _logger.LogWarning("Request was canceled - possible timeout or abort.");
            }
            if (ex.InnerException != null)
            {
                _logger.LogError("Inner exception: {Inner}", ex.InnerException.Message);
            }

            // Return empty list to avoid bubbling exceptions to the UI lifecycle.
            return new List<UserProfileDTO>();
        }
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

    public async Task<HttpResponseMessage> UpdateProfileAsync(Guid UserID, UserProfileDTO dto)
    {
        try
        {
            return await _http.PutAsJsonAsync($"api/Users/{UserID}", dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to Update Profile via POST {Endpoint}", "api/Profile");
            var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent(ex.Message)
            };
            return resp;
        }
    }

    public async Task<HttpResponseMessage> CreateUserAsync(IdentityDTO dto)
    {
        try
        {
            return await _http.PostAsJsonAsync("api/Identity", dto);
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

    public async Task<PreAuthResponseDTO?> LoginAsync(LoginDTO dto)
    {
        try
        {
            var result = await _http.PostAsJsonAsync("api/Identity/login", dto);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<PreAuthResponseDTO>();
            }

            _logger.LogWarning("Login failed with status {StatusCode}", result.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to login via POST {Endpoint}", "api/Identity/login");
            return null; // let the caller handle the failure
        }
    }

    public async Task<AuthDTO?> VerifyOtpAsync(Login2FADTO dto)
    {
        try
        {
            var result = await _http.PostAsJsonAsync("api/Identity/login-2fa", dto);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<AuthDTO>();
            }

            _logger.LogWarning("OTP verification failed with status {StatusCode}", result.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to verify OTP via POST {Endpoint}", "api/Identity/login-2fa");
            return null;
        }
    }

}

