using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.ViewModel
{
    public class EditUserViewModel
    {
        [Required, MaxLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, MaxLength(25)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile ProfilePhoto { get; set; }
    }
}
