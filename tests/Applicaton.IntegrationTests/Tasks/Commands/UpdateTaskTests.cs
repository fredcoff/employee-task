using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Tasks.Commands.CreateTask;
using EmployeeTask.Application.Tasks.Commands.UpdateTask;
using EmployeeTask.Application.Employees.Commands.CreateEmployee;
using EmployeeTask.Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;
using System;

namespace EmployeeTask.Application.IntegrationTests.Tasks.Commands
{
    using static Testing;

    public class UpdateTaskTests : TestBase
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

            var command = new UpdateTaskCommand
            {
                Id = itemId,
                Title = "Updated Task"
            };

            await SendAsync(command);

            var item = await FindAsync<Domain.Entities.Task>(itemId);

            item.Title.Should().Be(command.Title);
            item.LastModifiedBy.Should().NotBeNull();
            item.LastModifiedBy.Should().Be(userId);
            item.LastModified.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}
