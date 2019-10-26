using chat.backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chat.backend.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        private UserManager<ChatUser> _userManager;
        public HomeController(UserManager<ChatUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> User()
        {
            var userEmail = HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(userEmail.Value);

            return new OkObjectResult(new
            {
                user.ChatUserID,
                user.Email,
                user.UserName
            });
        }
    }
}
