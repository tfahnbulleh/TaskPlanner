using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(25)]
        public string TaskName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
