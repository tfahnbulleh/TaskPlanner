using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.ViewModel
{
    public class CreateTaskViewModel
    {
        [Required]
        [MaxLength(25)]
        public string TaskName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        public DateTime DueDate { get; set; }
    }
}
