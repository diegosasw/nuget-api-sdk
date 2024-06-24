namespace ApiSdk.Common;

public class TokenProviderException
    : Exception
{
    public TokenProviderException(string message, Exception exception)
        : base(message, exception)
    {
    }
}