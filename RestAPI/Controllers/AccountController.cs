using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        //TODO: add auth notification
        [HttpPost, Route("SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                if (model != null && ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (result.Succeeded)
                        {
                            return Ok();
                        }
                    }
                }
            }
            ModelState.AddModelError("Login", "Invalid login attempt;");

            return BadRequest();
        }

        [HttpGet, Route("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        //TODO: confirmation reg. by email with SendGrid
        [HttpPost, Route("SignUp")]
        public async Task<IActionResult> SignUpAsync(
            [FromBody] SignUpViewModel model
            )
        {
            if (ModelState.IsValid && model != null && model.Password == model.ConfirmPassword)
            {
                var user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.UpdateAsync(user);

                    await _signInManager.PasswordSignInAsync(user, model.Password, model.StayLoggedIn, false);

                    return Ok();
                }
            }

            ModelState.AddModelError(string.Empty, "Unsuccessful registration attempt");

            return BadRequest();
        }
    }
}
