using EmployeeTask.Application.Tasks.Commands.CreateTask;
using EmployeeTask.Application.Tasks.Commands.DeleteTask;
using EmployeeTask.Application.Tasks.Commands.UpdateTask;
using EmployeeTask.Application.Tasks.Commands.UpdateTaskDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeTask.WebUI.Controllers
{
    [Authorize]
    public class TasksController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateTaskCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTaskCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateTaskDetails(int id, UpdateTaskDetailCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTaskCommand { Id = id });

            return NoContent();
        }
    }
}
