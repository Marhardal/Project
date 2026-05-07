using FrontEnd.Client.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class ReviewService
    {
        private readonly HttpClient _http;
        private readonly ILogger<ProjectService> _logger;

        public ReviewService(HttpClient http, ILogger<ProjectService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<ReviewDTO>> GetReviewsAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<ReviewDTO>>($"api/Reviews");
                return result ?? new List<ReviewDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to GET reviews from {BaseAddress}{Endpoint}", _http.BaseAddress, "api/Reviews");
                return new List<ReviewDTO>();
            }
        }

        public async Task<ReviewDTO> GetReviewAsync(Guid id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ReviewDTO>($"api/Reviews/{id}");
                return result ?? new ReviewDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to GET review from {BaseAddress}{Endpoint}", _http.BaseAddress, $"api/Reviews/{id}");
                return new ReviewDTO();
            }
        }

        public async Task<HttpResponseMessage> CreateReviewAsync(ReviewDTO dto)
        {
            try
            {
                return await _http.PostAsJsonAsync("api/reviews", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create review via POST {Endpoint}", "api/reviews");
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }

    }
}
