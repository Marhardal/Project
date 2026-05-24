using System;
using ApexCharts;
using FrontEnd.Client.DTOs;
using FrontEnd.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddTransient<BearerTokenHandler>();
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register a named HttpClient and typed clients that use the BearerTokenHandler
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("API base URL not configured"));
    client.Timeout = TimeSpan.FromSeconds(100);
})
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));
builder.Services.AddApexCharts();
// Register typed clients so services receive configured HttpClient instances
builder.Services.AddHttpClient<ProponentService>((sp, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(100);
}).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<ContactPersonService>((sp, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(100);
}).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<ProjectService>((sp, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(100);
}).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<StatusService>((sp, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(100);
}).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<UserProfileService>((sp, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(100);
}).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<ReviewService>((sp, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(100);
}).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<HomeService>((sp, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(100);
}).AddHttpMessageHandler<BearerTokenHandler>();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<NavigationHistoryService>();
await builder.Build().RunAsync();
