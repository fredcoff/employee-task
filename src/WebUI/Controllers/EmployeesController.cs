using EmployeeTask.Application.Employees.Commands.CreateEmployee;
using EmployeeTask.Application.Employees.Commands.DeleteEmployee;
using EmployeeTask.Application.Employees.Commands.UpdateEmployee;
using EmployeeTask.Application.Employees.Queries.ExportTodos;
using EmployeeTask.Application.Employees.Queries.GetTodos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeTask.WebUI.Controllers
{
    [Authorize]
    public class EmployeesController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<TodosVm>> Get()
        {
            return await Mediator.Send(new GetTodosQuery());
        }

        [HttpGet("{id}")]
        public async Task<FileResult> Get(int id)
        {
            var vm = await Mediator.Send(new ExportTodosQuery { EmployeeId = id });

            return File(vm.Content, vm.ContentType, vm.FileName);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateEmployeeCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateEmployeeCommand command)
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
            await Mediator.Send(new DeleteEmployeeCommand { Id = id });

            return NoContent();
        }
    }
}
