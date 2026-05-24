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
builder.Services.AddScoped<BearerTokenHandler>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("API base URL not configured")) });
builder.Services.AddApexCharts();
builder.Services.AddScoped<ProponentService>();
builder.Services.AddScoped<ContactPersonService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<HomeService>(); 
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<NavigationHistoryService>();
await builder.Build().RunAsync();
