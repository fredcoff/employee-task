using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Tasks.Commands.CreateTask;
using EmployeeTask.Application.Tasks.Commands.UpdateTask;
using EmployeeTask.Application.Tasks.Commands.UpdateTaskDetail;
using EmployeeTask.Application.Employees.Commands.CreateEmployee;
using EmployeeTask.Domain.Entities;
using EmployeeTask.Domain.Enums;
using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;
using System;

namespace EmployeeTask.Application.IntegrationTests.Tasks.Commands
{
    using static Testing;

    public class UpdateTaskDetailTests : TestBase
    {
        [Test]
        public void ShouldRequireValidTaskId()
        {
            var command = new UpdateTaskCommand
            {
                Id = 99,
                Title = "Updated Task"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldUpdateTask()
        {
            var userId = await RunAsDefaultUserAsync();

            var employeeId = await SendAsync(new CreateEmployeeCommand
            {
                Name = "New Employee"
            });

            var itemId = await SendAsync(new CreateTaskCommand
            {
                EmployeeId = employeeId,
                Title = "New Task"
            });

            var command = new UpdateTaskDetailCommand
            {
                Id = itemId,
                EmployeeId = employeeId,
                Description = "This is the note.",
                Priority = PriorityLevel.Low
            };

            await SendAsync(command);

            var item = await FindAsync<Domain.Entities.Task>(itemId);

            item.EmployeeId.Should().Be(command.EmployeeId);
            item.Description.Should().Be(command.Description);
            item.Priority.Should().Be(command.Priority);
            item.LastModifiedBy.Should().NotBeNull();
            item.LastModifiedBy.Should().Be(userId);
            item.LastModified.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}
