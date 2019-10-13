using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using chat.backend.Models;
using chat.backend.Models.ViewModel;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Net;

namespace chat.backend.Controllers
{
    [Route("api/[controller]")]
    public class ChatUserController : ControllerBase
    {
        public UserManager<ChatUser> _userManager { get; set; }
        public SignInManager<ChatUser> _signInManager { get; set; }
        public ChatUserController(UserManager<ChatUser> userManager, SignInManager<ChatUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            ChatUser user = _userManager.Users.Where(x => x.ChatUserID == id).FirstOrDefault();  // FindByIdAsync(chatUserID);
            if(user == null)
            {
                return NotFound();
            }
            GetChatUserViewModel model = new GetChatUserViewModel { ChatUserID = user.ChatUserID, Email = user.Email, UserName = user.UserName };

            return Ok(model);

        }

        [HttpPost]
        [Route("Register")]
        //POST : /api/ChatUser/Register
        public async Task<IActionResult> Post([FromBody]CreateChatUserViewModel model)
        {
            ChatUser chatUser = new ChatUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(chatUser, model.Password);

            if (result.Succeeded)
            {
                chatUser = _userManager.Users.Where(u => u.Email == model.Email).FirstOrDefault();
                var route = RouteData;
                var uri = await Dns.GetHostEntryAsync("");
                return Created(uri.AddressList[3].ToString() + "/api/ChatUser/" + chatUser.ChatUserID, new { });
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EditEmailChatUserViewModel model)
        {
            ChatUser chatUser = _userManager.Users.Where(u => u.ChatUserID == model.ChatUserID).FirstOrDefault();
            if (chatUser == null)
            {
                return NotFound();
            }

            if (chatUser.Email != model.OldEmail)
            {
                var changeToken = await _userManager.GenerateChangeEmailTokenAsync(chatUser, model.NewEmail);

                var result = await _userManager.ChangeEmailAsync(chatUser, model.NewEmail, changeToken);

                var uri = await Dns.GetHostEntryAsync("");

                if (result.Succeeded)
                {
                    return Created(uri.ToString() + chatUser.ChatUserID, new { });
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("error.Code", error.Description);
                    }
                }
                return BadRequest(ModelState);
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(long id)
        {
            ChatUser user = _userManager.Users.Where(x => x.ChatUserID == id).FirstOrDefault();
            if(user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("error.Code", error.Description);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
