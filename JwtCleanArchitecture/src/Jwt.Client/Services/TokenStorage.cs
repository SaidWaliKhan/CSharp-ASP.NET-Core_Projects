using Microsoft.JSInterop;

namespace Jwt.Client.Services;

// Wraps the browser's localStorage so the rest of the app never touches JS interop directly.
public class TokenStorage
{
    private const string AccessTokenKey = "accessToken";
    private const string RefreshTokenKey = "refreshToken";

    private readonly IJSRuntime _js;

    public TokenStorage(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SaveTokensAsync(string accessToken, string refreshToken)
    {
        await _js.InvokeVoidAsync("authStorage.setItem", AccessTokenKey, accessToken);
        await _js.InvokeVoidAsync("authStorage.setItem", RefreshTokenKey, refreshToken);
    }

    public async Task<string?> GetAccessTokenAsync()
        => await _js.InvokeAsync<string?>("authStorage.getItem", AccessTokenKey);

    public async Task<string?> GetRefreshTokenAsync()
        => await _js.InvokeAsync<string?>("authStorage.getItem", RefreshTokenKey);

    public async Task ClearAsync()
    {
        await _js.InvokeVoidAsync("authStorage.removeItem", AccessTokenKey);
        await _js.InvokeVoidAsync("authStorage.removeItem", RefreshTokenKey);
    }
}