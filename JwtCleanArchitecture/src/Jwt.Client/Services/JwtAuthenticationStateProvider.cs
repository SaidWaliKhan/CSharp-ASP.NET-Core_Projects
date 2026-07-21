using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Jwt.Client.Services;

// This is what powers <AuthorizeView> and [Authorize] everywhere in the Blazor app.
// It doesn't validate the token's signature (the browser can't verify HMAC-SHA256 safely
// anyway) - it just reads the claims out of a token it trusts because it just received it
// from our own API over HTTPS. The API is still the one source of truth for validation.
public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly TokenStorage _tokenStorage;
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    public JwtAuthenticationStateProvider(TokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenStorage.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(Anonymous);
        }

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), authenticationType: "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    // Call this right after a successful login/register so the whole UI re-renders as "authenticated"
    public void NotifyUserAuthentication(string accessToken)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(accessToken), authenticationType: "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
        => NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        => new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims;
}