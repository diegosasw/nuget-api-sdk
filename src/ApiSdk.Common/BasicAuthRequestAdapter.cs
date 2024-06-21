using Microsoft.Kiota.Abstractions.Authentication;

namespace ApiSdk.Common;

public class BasicAuthRequestAdapter
    : BaseBearerTokenAuthenticationProvider
{
    public BasicAuthRequestAdapter(IAccessTokenProvider accessTokenProvider) : base(accessTokenProvider)
    {
    }
}