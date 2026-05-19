using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _js;

    // ✅ This is what was missing
    private static readonly AuthenticationState _anonymous =
        new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public AuthStateProvider(IJSRuntime js) => _js = js;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");

            Console.WriteLine($"Token from localStorage: {token}");

            if (string.IsNullOrEmpty(token))
                return _anonymous;

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Auth error: {ex.Message}");
            return _anonymous;
        }
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)))
        );
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = Convert.FromBase64String(
            payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '='));
        var kv = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return kv!.Select(k => new Claim(k.Key, k.Value.ToString()!));
    }
}