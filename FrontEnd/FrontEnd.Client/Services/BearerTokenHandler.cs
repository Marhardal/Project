using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

    public class BearerTokenHandler : DelegatingHandler
    {
    private readonly IJSRuntime _js;
    private readonly NavigationManager _navigation;
    private readonly IHttpClientFactory _httpClientFactory;

    public BearerTokenHandler(IJSRuntime js, NavigationManager navigation, IHttpClientFactory httpClientFactory)
    {
        _js = js;
        _navigation = navigation;
        _httpClientFactory = httpClientFactory;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? token = null;

        try
        {
            token = await GetValidTokenAsync();
        }
        catch (InvalidOperationException) { }

        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var newToken = await TryRefreshAsync();
            if (newToken != null)
            {
                var retry = await CloneRequestAsync(request);
                retry.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                response = await base.SendAsync(retry, cancellationToken);
            }

            // Only redirect if refresh also failed
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try { await _js.InvokeVoidAsync("Storage.clearToken"); }
                catch (InvalidOperationException) { }

                _navigation.NavigateTo("/Account/login", forceLoad: true);
            }
        }

        return response;
    }

    private async Task<string?> TryRefreshAsync()
    {
        string? refreshToken = null;
        try { refreshToken = await GetValidTokenAsync(); }
        catch (InvalidOperationException) { return null; }

        if (string.IsNullOrWhiteSpace(refreshToken)) return null;

        try
        {
            // Use a plain client — NOT the named "API" client (that would re-enter this handler)
            var client = _httpClientFactory.CreateClient("NoAuth");
            var resp = await client.PostAsJsonAsync("api/identity/refresh", new { refreshToken });
            if (!resp.IsSuccessStatusCode) return null;

            var result = await resp.Content.ReadFromJsonAsync<RefreshResponse>();
            if (result == null) return null;

            await _js.InvokeVoidAsync("Storage.setTokens", result.AccessToken, result.RefreshToken);
            return result.AccessToken;
        }
        catch { return null; }
    }
   
    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage req)
    {
        var clone = new HttpRequestMessage(req.Method, req.RequestUri);
        foreach (var h in req.Headers)
            clone.Headers.TryAddWithoutValidation(h.Key, h.Value);

        if (req.Content != null)
        {
            var bytes = await req.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(bytes);
            foreach (var h in req.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
        }

        return clone;
    }
   
    private async Task<string?> GetValidTokenAsync()
    {
        string? token = null;
        try { token = await _js.InvokeAsync<string>("Storage.getToken"); }
        catch (InvalidOperationException) { return null; }

        if (string.IsNullOrWhiteSpace(token)) return null;

        // Check if token expires within the next 60 seconds
        if (IsTokenExpiringSoon(token, bufferSeconds: 60))
            return await TryRefreshAsync();

        return token;
    }

    private bool IsTokenExpiringSoon(string token, int bufferSeconds)
    {
        try
        {
            // JWT is base64 header.payload.signature
            var parts = token.Split('.');
            if (parts.Length != 3) return true;

            var payload = parts[1];
            // Fix base64 padding
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));
            var doc = System.Text.Json.JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("exp", out var expProp)) return true;

            var exp = DateTimeOffset.FromUnixTimeSeconds(expProp.GetInt64());
            return exp <= DateTimeOffset.UtcNow.AddSeconds(bufferSeconds);
        }
        catch { return true; }
    }

    private record RefreshResponse(string AccessToken, string RefreshToken);
}

