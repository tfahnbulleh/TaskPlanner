using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.ViewModel
{
    public class TaskListViewModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }

        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }

    }
}
