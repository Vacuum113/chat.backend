using Microsoft.IdentityModel.Tokens;

namespace chat.backend.Auth
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}
