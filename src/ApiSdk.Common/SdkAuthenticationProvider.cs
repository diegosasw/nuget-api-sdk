using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace ApiSdk.Common;

public class SdkAuthenticationProvider
    : IAuthenticationProvider
{
    private readonly IAccessTokenProvider _tokenProvider;

    public SdkAuthenticationProvider(IAccessTokenProvider tokenProvider)
        => _tokenProvider = tokenProvider;

    public async Task AuthenticateRequestAsync(
        RequestInformation request, 
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var uri = new Uri(request.URI.ToString());
        var token = await _tokenProvider.GetAuthorizationTokenAsync(uri, additionalAuthenticationContext, cancellationToken);
        request.Headers.Add("Authorization", $"Bearer {token}");
    }
}