using chat.backend.Auth.JWT;
using chat.backend.Auth.RefreshToken;
using chat.backend.Helpers;
using chat.backend.Models;
using chat.backend.Models.Entities;
using chat.backend.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chat.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<ChatUser> _userManager;
        private IJwtSigningEncodingKey _signingEncodingKey;
        private ApplicationDbContex _applicationDbContex;

        public AuthController(
            [FromServices]IJwtSigningEncodingKey signingEncodingKey, 
            UserManager<ChatUser> userManager,
            ApplicationDbContex applicationDbContex
            )
        {
            _signingEncodingKey = signingEncodingKey;
            _userManager = userManager;
            _applicationDbContex = applicationDbContex;
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

            var refToken = new RfrshToken().GenerateRefreshToken();

            _applicationDbContex.RefreshTokens.Add(new RefToken { Id = validateCredentials.Id, RefreshToken = refToken, ExpirationTime = DateTime.Now.AddDays(1)});
            _applicationDbContex.SaveChanges();

            var claims = await _userManager.GetClaimsAsync(validateCredentials);

            var token = JwtTokenProps.GetJwtSecurityToken(_signingEncodingKey, claims);
 
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new OkObjectResult(new { jwt, refToken });
        }

        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(RefresTokenVievModel model )
        {
            //var user = await _userManager.FindByIdAsync(model.UserId);
            var user = _applicationDbContex.Users.Include("Token").First(c => c.Id == model.UserId);

            var result = user.Token.RefreshToken == model.RefreshToken;
            if (result && DateTime.Now <= user.Token.ExpirationTime)
            {
                var claims = await _userManager.GetClaimsAsync(user);

                var token = JwtTokenProps.GetJwtSecurityToken(_signingEncodingKey, claims);

                var refToken = new RfrshToken().GenerateRefreshToken();

                user.Token.ExpirationTime = DateTime.Now.AddDays(1);
                _applicationDbContex.RefreshTokens.Update(user.Token);
                _applicationDbContex.SaveChanges();

                string jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return new OkObjectResult(jwt);
            }
            else
            {
                Errors.AddErrorToModelState("invalid_token", "Refresh token failed verification.", ModelState);
                return BadRequest(ModelState);
            }
        }
    }
}
