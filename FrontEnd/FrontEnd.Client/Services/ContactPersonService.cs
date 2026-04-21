using FrontEnd.Client.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class ContactPersonService
    {
        private readonly HttpClient _http;
        private readonly ILogger<ContactPersonService> _logger;

        public ContactPersonService(HttpClient http, ILogger<ContactPersonService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<ContactPersonDTO>> GetContactPersonsAsync(Guid ProponentID)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<ContactPersonDTO>>($"api/ContactPersons/{ProponentID}");
                return result ?? new List<ContactPersonDTO>();
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
                return new List<ContactPersonDTO>();
            }
        }

        public async Task<HttpResponseMessage> CreateContactPersonAsync(ContactPersonDTO dto)
        {
            try
            {
                return await _http.PostAsJsonAsync("api/ContactPersons", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create ContactPersons via POST {Endpoint}", "api/Proponents");
                // Return a synthetic failure response so callers can handle gracefully
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }

    }
}
