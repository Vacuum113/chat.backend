

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace chat.backend.Auth.JWT
{
    public static class JwtConfig
    {
        public static string SecretKey { get; set; }

        public static string AuthenticationScheme { get; set; }

        public static AuthenticationBuilder AddJwtBearerOptions(this AuthenticationBuilder builder, string authScheme, IJwtSigningDecodingKey signingDecodKey)
        {
            return builder.AddJwtBearer(
                authScheme,
                jwtBearerOptions =>
                {
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.RequireHttpsMetadata = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingDecodKey.GetKey(),

                        ValidateIssuer = true,
                        ValidIssuer = "ChatWebApi",

                        ValidateAudience = true,
                        ValidAudience = "Web",

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });
        }
    }
}
