﻿using chat.backend.Auth.JWT;
using chat.backend.Auth.RefreshToken;
using chat.backend.Data;
using chat.backend.Data.IdentityUserAsp;
using chat.backend.Helpers;
using chat.backend.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private UserManager<IdentUser> _userManager;
        private IJwtSigningEncodingKey _signingEncodingKey;
        private ApplicationDbContex _applicationDbContex;

        public AuthController(
            [FromServices]IJwtSigningEncodingKey signingEncodingKey,
            UserManager<IdentUser> userManager,
            ApplicationDbContex applicationDbContex
            )
        {
            _signingEncodingKey = signingEncodingKey;
            _userManager = userManager;
            _applicationDbContex = applicationDbContex;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validateCredentials = await _applicationDbContex.Users
                .FirstAsync(l => l.Email == credentials.Email);

            if (!(await _userManager.CheckPasswordAsync(validateCredentials, credentials.Password)))
            {
                Errors.AddErrorToModelState("wrong_password.", "Password failed verification.", ModelState);
                return BadRequest(ModelState);
            }

            var claims = await _userManager.GetClaimsAsync(validateCredentials);
            string jwt = new JwtSecurityTokenHandler()
                .WriteToken(JwtTokenProps.GetJwtSecurityToken(_signingEncodingKey, claims));
            var refToken = new RfrshToken().GenerateRefreshToken();

            if (validateCredentials.RefreshToken == null)
            {
                validateCredentials.RefreshToken = refToken;
                validateCredentials.ExpirationTime = DateTime.UtcNow.AddDays(1) ;
            }
            else
            {
                if (DateTime.Now > validateCredentials.ExpirationTime)
                {
                    return new OkObjectResult(new { jwt, validateCredentials.RefreshToken });
                }
                else
                {
                    validateCredentials.RefreshToken = refToken;
                    validateCredentials.ExpirationTime = DateTime.Now.AddDays(1);
                }
            }
            await _applicationDbContex.SaveChangesAsync();
            return new OkObjectResult(new { jwt, refToken });
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(RefresTokenVievModel model )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEmail = HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Email)
                .Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                Errors.AddErrorToModelState("invalid_jwt_token", "Failed to parse jwt token.", ModelState);
                return BadRequest(ModelState);
            }

            var user = await _applicationDbContex.Users
                .FirstAsync(l => l.Email == userEmail);

            if (DateTime.Now > user.ExpirationTime &&
                user.RefreshToken == model.RefreshToken)
            {
                var token = JwtTokenProps
                    .GetJwtSecurityToken(_signingEncodingKey, User.Claims);
                string jwt = new JwtSecurityTokenHandler()
                    .WriteToken(token);
                return new OkObjectResult(jwt);
            }
            else
            {
                Errors.AddErrorToModelState("invalid_refresh_token", "Refresh token failed verification.", ModelState);
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LogOff")]
        [AllowAnonymous]
        public async Task<IActionResult> Post()
        {
            var userEmail = HttpContext.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(userEmail.Value);

            user.RefreshToken = null;
            user.ExpirationTime = null;
            await _applicationDbContex.SaveChangesAsync();

            return Ok();
        }
    }
}
