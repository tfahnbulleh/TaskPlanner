using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.ViewModel
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }

        public string Token { get; set; }

        public bool IsUserVerify { get; set; }

        [Display(Name = "Profile Photo")]
        public byte[] ProfilePhoto { get; set; }
    }
}
