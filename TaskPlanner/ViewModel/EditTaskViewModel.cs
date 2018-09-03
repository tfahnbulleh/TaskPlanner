using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TaskPlanner.ViewModel
{
    public class EditTaskViewModel
    {
        [Required]
        public int TaskId { get; set; }

        [Required]
        [MaxLength(25)]
        public string TaskName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        public DateTime DueDate { get; set; }
    }
}
