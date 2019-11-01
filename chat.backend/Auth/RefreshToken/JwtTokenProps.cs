using chat.backend.Auth.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace chat.backend.Auth.RefreshToken
{
    public static class JwtTokenProps
    {
        public static JwtSecurityToken GetJwtSecurityToken(IJwtSigningEncodingKey signingEncodingKey, IEnumerable<Claim> claims)
        {
            return new JwtSecurityToken(
                    issuer: "ChatWebApi",
                    audience: "Web",
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: new SigningCredentials(
                        signingEncodingKey.GetKey(),
                        signingEncodingKey.SigningAlgorithm)
                    );
        }
    }
}
