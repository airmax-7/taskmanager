using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TaskManagerAPI.Models.DTO;
using model = TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Authorize]
    public class TasksController : BaseController
    {
        // POST api/tasks
        [HttpPost]
        public async Task<IHttpActionResult> Post(TaskDTO newTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                model.Task task = new model.Task()
                {
                    Title = newTask.Title,
                    Description = newTask.Description,
                    BoardId = newTask.BoardId
                };
                var result = context.Tasks.Add(task);
                await context.SaveChangesAsync();
                return Ok(newTask);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(TaskAssignDTO dto)
        {
            try
            {
                //find the new assignee
                var user = context.Users.FirstOrDefault(u => u.Email == dto.AssigneeEmail);
                if(user != null)
                {
                    //set as new assignee
                    var task = await context.Tasks.FindAsync(dto.TaskId);
                    task.Assignees.Add(user);
                    await context.SaveChangesAsync();

                    //publish notification here

                    return Ok(task);
                }
                return NotFound();
            }

            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}