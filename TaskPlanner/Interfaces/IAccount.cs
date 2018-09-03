using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.Models;
using TaskPlanner.ViewModel;

namespace TaskPlanner.Interfaces
{
  public  interface IAccount
    {
        Task<IdentityResult> CreateAsync(RegisterViewModel model);
        Task<IdentityResult> EditUserProfile(EditUserViewModel viewModel);
        Task SendConfirmationEmailAsync(string callBackUrl, string email);
        Task<IdentityResult> ConfirmEmailAsync(string code, string userId);
        Task<IdentityResult> ChangePassword(ChangePasswordViewModel viewModel);
        Task<UserViewModel> LogIn(LogInViewModel viewModel);
        Task<IdentityResult> ResetPassword(ApplicationUser user, string code, string password);
        Task<ApplicationUser> FindByEmail(string email);
        Task<ApplicationUser> GetUser(System.Security.Claims.ClaimsPrincipal principle);
        Task<string> GenerateEmailConfirmationToken(ApplicationUser user);
        Task<bool> IsEmailConfirm(ApplicationUser user);
        Task<bool> IsEmailConfirm(string email);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
    }
}
