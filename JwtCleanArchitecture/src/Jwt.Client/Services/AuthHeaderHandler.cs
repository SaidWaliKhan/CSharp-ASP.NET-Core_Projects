using System.Net.Http.Headers;

namespace Jwt.Client.Services;

// A DelegatingHandler sits in front of every HttpClient call. This one stamps
// "Authorization: Bearer <token>" onto every outgoing request automatically,
// so individual pages never have to think about it.
public class AuthHeaderHandler : DelegatingHandler
{
    private readonly TokenStorage _tokenStorage;

    public AuthHeaderHandler(TokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenStorage.GetAccessTokenAsync();
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}