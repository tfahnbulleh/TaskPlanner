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
    [Route("Account/AccountAPI")]
    public class AccountAPIController : ControllerBase
    {
        //private readonly UserManager<ApplicationUser> _userManager;
       // private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IAccount  _accountRepository;

        public AccountAPIController(UserManager<ApplicationUser> userManager, 
            IEmailSender emailSender, 
            ApplicationDbContext context,
            ILogger<AccountAPIController> logger,
            IConfiguration configuration,
            IAccount account)
        {
           // _userManager = userManager;   
           // _emailSender = emailSender;
            _logger = logger;
            //  _accountRepository = new AccountRepository(userManager,context,emailSender,configuration);
            _accountRepository = account;
        }

        private async Task<string> GenerateEmailConfirmationLinkAsync(ApplicationUser user)
        {
            //generate yhr email confirmation token
            var code = await _accountRepository.GenerateEmailConfirmationToken(user);
            //call back url to sent to the email
            var callbackUrl = Url.Action("ConfirmEmail", "AccountAPI", new { userId = user.Id, code = code }, Request.Scheme);
            return callbackUrl;
        }

        [HttpPost, Route("Register")]
        public async Task<ActionResult> Register([FromBody]RegisterViewModel model)
        {
            //if model is valid
            if (ModelState.IsValid) 
            {
                //the value of password and confirm password is not the same
                if (model.ConfirmPassword != model.Password)
                {
                    ModelState.AddModelError("UnMatchPassword", "Password does not match");
                    return BadRequest(ModelState);
                }

                //method call to create new user
                var result = await _accountRepository.CreateAsync(model);
                if (result.Succeeded)
                {
                    //log 
                    _logger.LogInformation("User created a new account with password.");

                    //find the user by email
                    var user = await _accountRepository.FindByEmail(model.Email);

                    //generate yhr email confirmation token
                    var code = await _accountRepository.GenerateEmailConfirmationToken(user);

                    //call back url to sent to the email
                    var callbackUrl =await this.GenerateEmailConfirmationLinkAsync(user);

                    //send email to user
                    await _accountRepository.SendConfirmationEmailAsync(callbackUrl, user.Email);
                    // await _signInManager.SignInAsync(user, false);
                    return new JsonResult(Created("Account/AccountApi/Register", "Account created successfully"));
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                BadRequest(ModelState);
            }
            // If we got this far, something failed, redisplay form
            return BadRequest(ModelState);
        }

        [HttpPost, Route("login")]
        public async Task<ActionResult> LogIn([FromBody]LogInViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result =await _accountRepository.LogIn(viewModel);
                return new JsonResult(Ok(result));
            }
            catch ( Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("sendemailconfirmationlink/{email}")]
        [Authorize]
        public async Task<IActionResult> SendEmailConfirmationLink([FromRoute]string email)
        {
            var con = this;
            var user = await _accountRepository.GetUser(User);

            if (user.Email != email)
            {
                return BadRequest("Invalid data received");
            }
            if (user == null) //user is null
            {
                return new JsonResult(Ok("email sent"));
            }

            //user is not null
            else
            {
                
                try
                {
                    var callBackUrl = await this.GenerateEmailConfirmationLinkAsync(user);
                    //method call to send email confirmation link
                    await _accountRepository.SendConfirmationEmailAsync(callBackUrl, user.Email);
                    return new JsonResult(Ok("email sent"));
                }
                catch (Exception)
                {
                    return new JsonResult("an error occur on the server");
                }
              
            }
        }


        //verify user email
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            var result = await _accountRepository.ConfirmEmailAsync(code, userId);

            if (result.Succeeded)
            {
                return Content("Email Account Successfully confirmed!!!");
            }
            else
            {
                return BadRequest("Failed to confirm email");
            }
        }





    }
}