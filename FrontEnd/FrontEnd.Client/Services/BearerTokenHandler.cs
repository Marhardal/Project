using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Headers;

    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;
    private readonly NavigationManager navigation;

        public BearerTokenHandler(IJSRuntime js, NavigationManager navigationManager)
        {
            _js = js;
            navigation = navigationManager;
        }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? token = null;

        try
        {
            token = await _js.InvokeAsync<string>("Storage.getToken");
        }
        catch (InvalidOperationException)
        {
            // JS interop not available yet (prerendering) — proceed without token
        }

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            try
            {
                await _js.InvokeVoidAsync("Storage.removeToken");
            }
            catch (InvalidOperationException) { }

            navigation.NavigateTo("/Account/login", true);
        }

        return response;
    }
}

