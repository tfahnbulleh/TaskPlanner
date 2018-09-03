using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.Models;

namespace TaskPlanner.Interfaces
{
  public  interface ITaskRepository 
    {
        void CreateAsync(TaskModel model);
        void EditAsync(TaskModel model);
        Task<TaskModel> FindAsync(int id);
        Task<IEnumerable<TaskModel>> GetListAsync(ApplicationUser user);
        void DeleteAsync(int id);
        Task<IEnumerable<TaskModel>> GetCompletedTasksAsync(ApplicationUser user);
        Task<IEnumerable<TaskModel>> GetUnCompletedTasksAsync(ApplicationUser user);
        int Count();
        Task<bool> IsTaskExistAsync(int id);
       
    }
}
