using FrontEnd.Client.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace FrontEnd.Client.Services
{
    public class TasksService
    {
        private readonly HttpClient _http;
        private readonly ILogger<TasksService> _logger;

        public TasksService(HttpClient http, ILogger<TasksService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<TasksDTO>> GetProjectTasks(Guid ProjectID)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<TasksDTO>>($"api/Tasks/{ProjectID}");
                return result ?? new List<TasksDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to GET Tasks from {BaseAddress}{Endpoint}", _http.BaseAddress, "api/Tasks");
                return new List<TasksDTO>();
            }
        }

        public async Task<HttpResponseMessage> CreateTaskAsync(TasksDTO dto)
        {
            try
            {
                return await _http.PostAsJsonAsync("api/Tasks", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Tasks via POST {Endpoint}", "api/Tasks");
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }

        public async Task<HttpResponseMessage> UpdateTaskAsync(Guid id, TasksDTO dto)
        {
            try
            {
                return await _http.PostAsJsonAsync($"api/Tasks/{id}", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Tasks via POST {Endpoint}", "api/Tasks");
                var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(ex.Message)
                };
                return resp;
            }
        }

    }
}
