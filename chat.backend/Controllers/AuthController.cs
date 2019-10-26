using chat.backend.Auth;
using chat.backend.Helpers;
using chat.backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chat.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ChatUser> _userManager;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;
        public AuthController([FromServices]IJwtSigningEncodingKey signingEncodingKey, UserManager<ChatUser> userManager)
        {
            _signingEncodingKey = signingEncodingKey;
            _userManager = userManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validateCredentials = await _userManager.FindByEmailAsync(credentials.Email);
            if (!(await _userManager.CheckPasswordAsync(validateCredentials, credentials.Password)))
            {
                Errors.AddErrorToModelState("Wrong password.", "Password failed verification.", ModelState);
                return BadRequest(ModelState);
            }

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, credentials.Email),
                new Claim(Constants.JwtClaimIdentifiers.Rol, Constants.JwtClaims.ApiAccess),
                new Claim("id", validateCredentials.Id)
            };

            var token = new JwtSecurityToken(
                issuer: "ChatWebApi",
                audience: "Web",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: new SigningCredentials(
                    _signingEncodingKey.GetKey(),
                    _signingEncodingKey.SigningAlgorithm)
                );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new OkObjectResult(jwt);
        }
    }
}
