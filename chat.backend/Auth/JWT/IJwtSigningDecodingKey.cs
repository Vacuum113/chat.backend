using Microsoft.IdentityModel.Tokens;

namespace chat.backend.Auth.JWT
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
