using FrontEnd.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("API base URL not configured")) });

builder.Services.AddScoped<ProponentService>();
builder.Services.AddScoped<ContactPersonService>();
await builder.Build().RunAsync();
