using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaskPlanner.CSFiles.Repositories;
using TaskPlanner.Data;
using TaskPlanner.Interfaces;
using TaskPlanner.Models;
using TaskPlanner.Services;
using TaskPlanner.ViewModel;

namespace TaskPlanner.Controllers.APIControllers
{
    [Produces("application/json")]
    [Route("Account/ManageAccount")]
    public class ManageAccountAPIController : Controller
    {
       // private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IAccount _accountRepository;

        public ManageAccountAPIController(UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            ILogger<ManageAccountAPIController> logger,
            ApplicationDbContext context,
            IConfiguration configuration,
            IAccount account)
        {
           // _emailSender = emailSender;
            _logger = logger;
            _accountRepository = account;
        }

        [Authorize]
        [HttpGet, Route("{userName}")]
        public async Task<IActionResult> GetUser([FromRoute] string userName)
        {
            var user = await _accountRepository.GetUser(User);
            if (user == null || user.Email != userName)
            {
                return Unauthorized();
            }
            var result = new UserViewModel { FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName, IsUserVerify = user.EmailConfirmed };
            return Json(Ok(result));
        }

        [HttpPut("{userName}")]
        [Authorize]
        public async Task<IActionResult> Put([FromRoute]string userName, [FromBody] EditUserViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userName != viewModel.UserName)
            {
                ModelState.AddModelError("unmatchedData", "User does not match");
                return BadRequest(ModelState);
            }

            var user = await _accountRepository.GetUser(User);
            if (!user.EmailConfirmed || user.Email!=viewModel.UserName)
            {
                return Unauthorized();
            }

            try
            {
                var result = await _accountRepository.EditUserProfile(viewModel);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                return new JsonResult(Ok("Success"));
            }
            catch (Exception ex)
            {
                return new JsonResult(StatusCode(500, "An unknow error occur"));
            }
        }

        
        [Authorize]
        [HttpPost, Route("chnage-my-password")]
       public async Task<ActionResult> ChangePassword([FromBody]ChangePasswordViewModel changePasswordViewModel)
        {
            if (!ModelState.IsValid) //model is not valid
            {
                return BadRequest(ModelState);
            }

            //confirn new password and new password is not the same
            if (changePasswordViewModel.ConfirmNewPassword!=changePasswordViewModel.NewPassword)
            {
                ModelState.AddModelError("PasswordUnmatched", "The new password does not match the confirm password");
                return BadRequest(ModelState);
            }

            //get the sign in user
            var user = await _accountRepository.GetUser(User);

            //user is null
            if (user==null || user.Email!=changePasswordViewModel.Email)
            {
                return Unauthorized();
            }

            //method call to change the user password
            var result =await _accountRepository.ChangePassword(changePasswordViewModel);

            //result succeeded
            if (result.Succeeded)
            {
                return new JsonResult(Ok("Password change successfully"));
            }

            //result did not succceed
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(err.Code, err.Description);
            }
            return BadRequest(ModelState);        
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

    }
}