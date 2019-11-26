using chat.backend.Data.IdentityUserAsp;
using chat.backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chat.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        public UserManager<IdentUser> _userManager { get; set; } 

        public AccountController(UserManager<IdentUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        //POST : /api/Account/Register
        public async Task<IActionResult> Post([FromBody]CreateChatUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identUser = new IdentUser{ UserName = model.UserName, Email = model.Email};
            var result = await _userManager.CreateAsync(identUser, model.Password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(Constants.JwtClaimIdentifiers.Rol, Constants.JwtClaims.User),
                    new Claim("id", user.Id)
                };

                result = await _userManager.AddClaimsAsync(user, claims);
                if (result.Succeeded)
                {
                    return new OkObjectResult("Account created");
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                    Errors.AddErrorsToModelState(result, ModelState);
                }
            }
            else
            {
                Errors.AddErrorsToModelState(result, ModelState);
            }
            return new BadRequestObjectResult(ModelState);
        }
    }
}
