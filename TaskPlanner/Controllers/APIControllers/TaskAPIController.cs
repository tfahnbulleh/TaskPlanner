using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskPlanner.Data;
using TaskPlanner.Interfaces;
using TaskPlanner.Models;
using TaskPlanner.ViewModel;

namespace TaskPlanner.Controllers.APIControllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("task/TaskAPI")]
    public class TaskAPIController : Controller
    {
       // private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskRepository _taskRepository;
        public TaskAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ITaskRepository taskRepository)
        {
            _userManager = userManager;
            _taskRepository = taskRepository;
        }

        // GET: api/TaskAPI
        [HttpGet, Route("getcompletedtask")]
        public async Task<IEnumerable<TaskListViewModel>> GetCompletedTasksAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = await _taskRepository.GetCompletedTasksAsync(user);
            var result = (from t in tasks
                          select new TaskListViewModel
                          {
                              Description = t.Description,
                              TaskName = t.TaskName,
                              DueDate = t.DueDate,
                              IsCompleted = t.IsCompleted,
                              TaskId = t.TaskId
                          }).AsEnumerable();
            return result;
        }

        [HttpGet, Route("getuncompletedtask")]
        public async Task<IEnumerable<TaskListViewModel>> GetUnCompletedTasksAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = await _taskRepository.GetUnCompletedTasksAsync(user);
            var result = (from t in tasks
                          select new TaskListViewModel
                          {
                              Description = t.Description,
                              TaskName = t.TaskName,
                              DueDate = t.DueDate,
                              IsCompleted = t.IsCompleted,
                              TaskId = t.TaskId
                          }).AsEnumerable();
            return result;
        }

        // GET: api/TaskAPI
        [HttpGet]
        public async Task<IEnumerable<TaskListViewModel>> GetTasksAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = await _taskRepository.GetListAsync(user);
            var result = (from t in tasks
                          select new TaskListViewModel
                          {
                              Description = t.Description,
                              TaskName = t.TaskName,
                              DueDate = t.DueDate,
                              IsCompleted = t.IsCompleted,
                              TaskId = t.TaskId
                          }).AsEnumerable();
            return result;
        }

        // GET: api/TaskAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskModel = await _taskRepository.FindAsync(id);

            if (taskModel == null)
            {
                return NotFound();
            }
            var result = new TaskListViewModel
            {
                Description = taskModel.Description,
                DueDate = taskModel.DueDate,
                IsCompleted = taskModel.IsCompleted,
                TaskId = taskModel.TaskId,
                TaskName = taskModel.TaskName
            };

            return Ok(result);
        }

        // PUT: api/TaskAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskModel([FromRoute] int id, [FromBody] EditTaskViewModel taskModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskModel.TaskId)
            {
                return BadRequest();
            }
            var model = new TaskModel
            {
                TaskId = taskModel.TaskId,
                Description = taskModel.Description,
                TaskName = taskModel.TaskName,
                DueDate = taskModel.DueDate
            };
            try
            {
               
                 _taskRepository.EditAsync(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                var isExist =await _taskRepository.IsTaskExistAsync(model.TaskId);
                if (!isExist)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Successfully updated");
        }

        // POST: api/TaskAPI
        [HttpPost]
        public async Task<IActionResult> PostTaskModel([FromBody] CreateTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user =await _userManager.GetUserAsync(User);
            TaskModel taskModel = new TaskModel
            {
                Description = model.Description,
                DueDate = model.DueDate,
                IsCompleted = false,
                TaskName = model.TaskName,
                UserId = user.Id
            };
            _taskRepository.CreateAsync(taskModel);
            return Created("task/taskAPI","Created successfully");
           // return CreatedAtAction("GetTaskModel", new { id = taskModel.TaskId }, taskModel);
        }

        // DELETE: api/TaskAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskModel = await _taskRepository.FindAsync(id);
            if (taskModel == null)
            {
                return NotFound();
            }

            try
            {
                _taskRepository.DeleteAsync(taskModel.TaskId);
                return Ok("Successfully deleted");
            }
            catch (Exception)
            {

                throw;
            }
            
        }

       
    }
}