using Jwt.Client;
using Jwt.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Point this at wherever your Jwt.API project is actually running (check its launchSettings.json)
var apiBaseAddress = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5001";

// AuthHeaderHandler intercepts every outgoing request and attaches "Authorization: Bearer <token>"
builder.Services.AddScoped<AuthHeaderHandler>();

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<AuthHeaderHandler>();

// Anything that just injects "HttpClient" gets the pre-configured "Api" client
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddScoped<ApiAuthService>();

await builder.Build().RunAsync();