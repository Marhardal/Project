using Microsoft.JSInterop;

    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;

        public BearerTokenHandler(IJSRuntime js)
        {
            _js = js;
        }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Use Storage.getToken which unwraps the token correctly
        var token = await _js.InvokeAsync<string>("Storage.getToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

