using System.Net.Http.Json;
using Jwt.Client.Models;

namespace Jwt.Client.Services;

// The single place that knows how to call /api/auth/* on Jwt.API.
// Pages never call HttpClient directly - they call this.
public class ApiAuthService
{
    private readonly HttpClient _http;
    private readonly TokenStorage _tokenStorage;
    private readonly JwtAuthenticationStateProvider _authStateProvider;

    public ApiAuthService(HttpClient http, TokenStorage tokenStorage, JwtAuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _tokenStorage = tokenStorage;
        _authStateProvider = authStateProvider;
    }

    public Task<AuthResult> LoginAsync(LoginModel model)
        => PostAuthRequestAsync("api/auth/login", model);

    public Task<AuthResult> RegisterAsync(RegisterModel model)
        => PostAuthRequestAsync("api/auth/register", model);

    public async Task LogoutAsync()
    {
        await _tokenStorage.ClearAsync();
        _authStateProvider.NotifyUserLogout();
    }

    private async Task<AuthResult> PostAuthRequestAsync<TRequest>(string endpoint, TRequest payload)
    {
        var response = await _http.PostAsJsonAsync(endpoint, payload);
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

        if (response.IsSuccessStatusCode && result is { Success: true, AccessToken: not null })
        {
            await _tokenStorage.SaveTokensAsync(result.AccessToken, result.RefreshToken ?? string.Empty);
            _authStateProvider.NotifyUserAuthentication(result.AccessToken);
            return new AuthResult(true, result.Message);
        }

        return new AuthResult(false, result?.Message ?? "Something went wrong. Please try again.");
    }
}

public record AuthResult(bool Success, string Message);