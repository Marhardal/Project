using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FrontEnd.DTOs;
using Microsoft.Extensions.Logging;

public class ProponentService
{
    private readonly HttpClient _http;
    private readonly ILogger<ProponentService> _logger;

    public ProponentService(HttpClient http, ILogger<ProponentService> logger)
    {
        _http = http;
        _logger = logger;
    }

    // ✅ Get all projects
    public async Task<List<ProponentsDTO>> GetProponentsAsync()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<ProponentsDTO>>("api/proponents");
            return result ?? new List<ProponentsDTO>();
        }
        catch (Exception ex)
        {
            // Log full exception including inner exceptions to help root-cause analysis
            _logger.LogError(ex, "Failed to GET proponents from {BaseAddress}{Endpoint}", _http.BaseAddress, "api/proponents");
            if (ex is TaskCanceledException)
            {
                _logger.LogWarning("Request was canceled - possible timeout or abort.");
            }
            if (ex.InnerException != null)
            {
                _logger.LogError("Inner exception: {Inner}", ex.InnerException.Message);
            }

            // Return empty list to avoid bubbling exceptions to the UI lifecycle.
            return new List<ProponentsDTO>();
        }
    }

    public async Task<HttpResponseMessage> CreateProponentAsync(ProponentsDTO dto)
    {
        try
        {
            return await _http.PostAsJsonAsync("api/Proponents", dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create proponent via POST {Endpoint}", "api/Proponents");
            // Return a synthetic failure response so callers can handle gracefully
            var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent(ex.Message)
            };
            return resp;
        }
    }
}

