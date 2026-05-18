using FrontEnd;
using FrontEnd.Client.DTOs;
using FrontEnd.Client.Services;
using FrontEnd.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// In FrontEnd/Program.cs (server)
builder.Services.AddAuthorization();          // ✅ full version for middleware pipeline
builder.Services.AddAuthorizationCore();      // ✅ for Blazor components
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

// Register a named HttpClient for general API use
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7120/");
    client.Timeout = TimeSpan.FromSeconds(100);
});

// Register ProponentService with a typed HttpClient so it receives a configured HttpClient instance
builder.Services.AddHttpClient<ProponentService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7120/");
    client.Timeout = TimeSpan.FromSeconds(100);
});

builder.Services.AddScoped<AuthStateService>();

//builder.Services.AddHttpClient<StatusService>((sp, client) =>
//{
//    client.BaseAddress = new Uri("https://localhost:7120/");
//    client.Timeout = TimeSpan.FromSeconds(100);
//});

builder.Services.AddHttpClient<ContactPersonService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7120/");
    client.Timeout = TimeSpan.FromSeconds(100);
});

builder.Services.AddHttpClient<ProjectService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7120/");
    client.Timeout = TimeSpan.FromSeconds(100);
});

builder.Services.AddHttpClient<StatusService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7120/");
    client.Timeout = TimeSpan.FromSeconds(100);
});

builder.Services.AddHttpClient<UserProfileService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7120/");
    client.Timeout = TimeSpan.FromSeconds(100);
});

builder.Services.AddHttpClient<ReviewService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7120/");
    client.Timeout = TimeSpan.FromSeconds(100);
});

builder.Services.AddHttpClient<HomeService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7120/");
    client.Timeout = TimeSpan.FromSeconds(100);
});

// Register CORS and a named policy used by the middleware later in the pipeline.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        // Allow the Blazor WebAssembly client origin and enable credentials
        policy.WithOrigins("https://localhost:7120")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7217")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ProponentService is provided via AddHttpClient<ProponentService>() above, which registers
// a typed client and ensures HttpClient is configured with BaseAddress. Do not re-register
// as a plain scoped service — that would bypass the typed client and provide an HttpClient
// with no BaseAddress (causing relative URI errors).

// builder.Services.AddScoped<ProponentService>(); // removed to use typed client

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

// Ensure CORS middleware runs before mapping endpoints so the policy is applied
app.UseCors("AllowBlazor");

app.UseCors("AllowFrontend");

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FrontEnd.Client._Imports).Assembly);

app.Run();
