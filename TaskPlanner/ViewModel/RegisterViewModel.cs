using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.ViewModel
{
    public class RegisterViewModel
    {
        [Required, MaxLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required, MaxLength(25)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password"), MinLength(8)]
        public string Password { get; set; }


        [Required]
        [Display(Name = "Confirm Password"), MinLength(8)]
        [Compare("Password",ErrorMessage ="Password does not match")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Profile Photo")]
        public IFormFile ProfilePhoto { get; set; }

    }
}
