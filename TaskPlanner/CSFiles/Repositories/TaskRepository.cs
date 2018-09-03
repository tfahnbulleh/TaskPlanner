using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.Data;
using TaskPlanner.Interfaces;
using TaskPlanner.Models;
using TaskPlanner.ViewModel;

namespace TaskPlanner.CSFiles.Repositories
{
    public class TaskRepository: UnitWork,ITaskRepository
    {
     
        public TaskRepository(ApplicationDbContext dbContext):base(dbContext)
        {   
          
        }

        public int Count()
        {
            var result= base.GetContext.Tasks.Count();
            return  result;
        }

        public void CreateAsync(TaskModel model)
        {
            base.GetContext.Tasks.Add(model);
            base.Save();
        }

      
        public async void DeleteAsync(int id)
        {
            var task = await this.FindAsync(id);
            base.GetContext.Tasks.Remove(task);
            await base.SaveAsync();
        }

        public void EditAsync(TaskModel model)
        {
            if (model.DueDate < DateTime.Now)
            {
                model.IsCompleted = true;
            }


            else
            {
                model.IsCompleted = false;
            }

            var result = base.GetContext.Tasks.Find(model.TaskId);
            result.TaskName = model.TaskName;
            result.DueDate = model.DueDate;
            result.Description = model.Description;
            result.IsCompleted = model.IsCompleted;
            GetContext.Update(result);
            base.Save();
        }

        public async Task<TaskModel> FindAsync(int id)
        {
            var task =  await base.GetContext.Tasks.FindAsync(id);
            return task;
        }

        public async Task<IEnumerable<TaskModel>> GetCompletedTasksAsync(ApplicationUser user)
        {
            var result = await base.GetContext.Tasks.Where(m =>m.UserId.Equals(user.Id) &&  m.IsCompleted == true).ToListAsync();

            await UpdateIsTaskCompleted(result);
            return result;
        }

        public  async Task<IEnumerable<TaskModel>> GetListAsync(ApplicationUser user)
        {
            var tasks =await base.GetContext.Tasks.Where(m => m.UserId.Equals(user.Id)).ToListAsync();
            return tasks.AsEnumerable();
        }

        public async Task<IEnumerable<TaskModel>> GetUnCompletedTasksAsync(ApplicationUser user)
        {
            var result = await base.GetContext.Tasks.Where(m => m.UserId.Equals(user.Id) && m.IsCompleted == false).ToListAsync();
            await UpdateIsTaskCompleted(result);
            return result;
        }

        public async Task<bool> IsTaskExistAsync(int id)
        {
            var result =await  base.GetContext.Tasks.Where(m => m.TaskId == id).AnyAsync();
            return result;
        }

        private async Task UpdateIsTaskCompleted(IEnumerable<TaskModel> list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                if (list.ElementAt(i).DueDate < DateTime.Now)
                {
                    list.ElementAt(i).IsCompleted = true;
                }
            }
            base.GetContext.UpdateRange(list);
           await base.SaveAsync();
        }
    }
}
