using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TaskPlanner.Data;
using TaskPlanner.Interfaces;
using TaskPlanner.Models;
using TaskPlanner.Services;
using TaskPlanner.ViewModel;

namespace TaskPlanner.CSFiles.Repositories
{
    public class AccountRepository: IAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Common _common;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public AccountRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _emailSender = emailSender;
            _common = new Common();
            _config = configuration;
        }

        //find the user by email
        public async Task<ApplicationUser> FindByEmail(string email)
        {
            var user =await _userManager.FindByEmailAsync(email);
            return user;
        }

        //get the user using the claims principle
        public async Task<ApplicationUser> GetUser(System.Security.Claims.ClaimsPrincipal principle)
        {
            var user = await _userManager.GetUserAsync(principle);
            return user;
        }

        //generate email confirmation token for user to confirm their email
        public async Task<string>GenerateEmailConfirmationToken(ApplicationUser user)
        {
            var token =await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        //create new user
        public async Task<IdentityResult> CreateAsync(RegisterViewModel viewModel)
        {
            
            //new AppUser object
            var user = new ApplicationUser
            {
                Email = viewModel.Email,
                UserName = viewModel.Email,
                LastName = viewModel.LastName,
                FirstName = viewModel.FirstName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            //create user account
            var result = await _userManager.CreateAsync(user, viewModel.Password);
            return result;
        }


        //edit user profile
        public async Task<IdentityResult> EditUserProfile(EditUserViewModel viewModel)
        {
            var appUser = _dbContext.Users.Where(m => m.Email.Equals(viewModel.UserName)).Single();
            _dbContext.Entry(appUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            appUser.FirstName = viewModel.FirstName;
            appUser.LastName = viewModel.LastName;
            await _dbContext.SaveChangesAsync();
            return IdentityResult.Success;
        }

        //send user email confirmation link
        public async Task SendConfirmationEmailAsync(string callBackUrl, string email)
        {
            await _emailSender.SendEmailAsync(email, "Confirm your email",
                     $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>clicking here</a>.");
        }

        //this method is called when the user click the link to confirm their email 
        public async Task<IdentityResult> ConfirmEmailAsync(string code, string userId)
        {
            List<IdentityError> error = new List<IdentityError>();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    error.Add(new IdentityError { Code = "404", Description = "User not found" });
                    return IdentityResult.Failed(error.ToArray());
                }
                await _userManager.ConfirmEmailAsync(user, code);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                error.Add(new IdentityError { Code = "00", Description = ex.Message });
                return IdentityResult.Failed(error.ToArray());
            }

        }

        //method for user to change password
        public async Task<IdentityResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            List<IdentityError> error = new List<IdentityError>(); //store error list
            var user = await _userManager.FindByEmailAsync(viewModel.Email); //find the user

            if (user == null)//the user is null
            {
                error.Add(new IdentityError
                {
                    Description = "Unable to load user",
                    Code = "00"

                });
                return IdentityResult.Failed(error.ToArray());
            }

            //change the user password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.NewPassword);

            if (!changePasswordResult.Succeeded) //the change password was not successful
            {
                foreach (var err in changePasswordResult.Errors)
                {
                    error.Add(err);
                }
                return IdentityResult.Failed(error.ToArray());
            }
            //the change password was successful
            return IdentityResult.Success;
        }


        //log in a user
        //this is most likely use for api call
        public async Task<UserViewModel> LogIn(LogInViewModel viewModel)
        {
            //check for user
            var user = await _userManager.FindByEmailAsync(viewModel.UserName);

            //user does not exist
            if (user == null)
            {
                throw new NullReferenceException("Invalid user name or password");
            }

            //user exist
            //validate username with password 
            var result = await _userManager.CheckPasswordAsync(user, viewModel.Password);
            var tokenGenerator = new JWTTokenGenerator(this._config, user);
            //password match
            if (result)
            {
                return new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.Email,
                    IsUserVerify = user.EmailConfirmed,
                    Token = tokenGenerator.Token
                };
                
            }
            throw new InvalidOperationException("Invalid user name or password");
        }


        //reset user password
        public async Task<IdentityResult>ResetPassword(ApplicationUser user, string code, string password)
        {
            try
            {
                var result = await _userManager.ResetPasswordAsync(user, code, password);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //determine if user email is confifrm
        public async Task<bool> IsEmailConfirm(ApplicationUser user)
        {
            return  await _userManager.IsEmailConfirmedAsync(user);
        }

        //determine if user email is confirm
        public async Task<bool> IsEmailConfirm(string email)
        {
            var user =await this.FindByEmail(email);
            return user.EmailConfirmed;
        }

        //generate token to reset password
        public Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user);
        }
    }
}
