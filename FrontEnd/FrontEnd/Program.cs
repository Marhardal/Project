using FrontEnd;
using FrontEnd.Client.DTOs;
using FrontEnd.Client.Services;
using FrontEnd.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7217", "https://localhost:5273")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Server host should not register browser-only services (IJSRuntime-backed)
// Keep authorization for the server endpoints/middleware only
builder.Services.AddAuthorization();

// Register CORS and a named policy used by the middleware later in the pipeline.
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowBlazor", policy =>
//    {
//        // Allow the Blazor WebAssembly client origin and enable credentials
//        policy.WithOrigins("https://localhost:7120")
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials();
//    });
//});

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
//app.UseCors("AllowBlazor");

app.UseCors("AllowFrontend");

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FrontEnd.Client.Components._Imports).Assembly);

app.Run();
