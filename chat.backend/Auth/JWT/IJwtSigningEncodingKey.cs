using Microsoft.IdentityModel.Tokens;

namespace chat.backend.Auth.JWT
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}
