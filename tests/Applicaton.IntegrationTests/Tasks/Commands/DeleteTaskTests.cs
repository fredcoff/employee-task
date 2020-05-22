using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Tasks.Commands.CreateTask;
using EmployeeTask.Application.Tasks.Commands.DeleteTask;
using EmployeeTask.Application.Employees.Commands.CreateEmployee;
using EmployeeTask.Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EmployeeTask.Application.IntegrationTests.Tasks.Commands
{
    using static Testing;

    public class DeleteTaskTests : TestBase
    {
        [Test]
        public void ShouldRequireValidTaskId()
        {
            var command = new DeleteTaskCommand { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldDeleteTask()
        {
            var employeeId = await SendAsync(new CreateEmployeeCommand
            {
                Name = "New Employee"
            });

            var itemId = await SendAsync(new CreateTaskCommand
            {
                EmployeeId = employeeId,
                Title = "New Task"
            });

            await SendAsync(new DeleteTaskCommand
            {
                Id = itemId
            });

            var list = await FindAsync<Domain.Entities.Task>(employeeId);

            list.Should().BeNull();
        }
    }
}
