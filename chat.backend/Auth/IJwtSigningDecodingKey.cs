using Microsoft.IdentityModel.Tokens;

namespace chat.backend.Auth
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
