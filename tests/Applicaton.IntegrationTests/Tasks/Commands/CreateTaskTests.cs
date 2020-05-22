using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Tasks.Commands.CreateTask;
using EmployeeTask.Application.Employees.Commands.CreateEmployee;
using EmployeeTask.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EmployeeTask.Application.IntegrationTests.Tasks.Commands
{
    using static Testing;

    public class CreateTaskTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateTaskCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldCreateTask()
        {
            var userId = await RunAsDefaultUserAsync();

            var employeeId = await SendAsync(new CreateEmployeeCommand
            {
                Name = "New Employee"
            });

            var command = new CreateTaskCommand
            {
                EmployeeId = employeeId,
                Title = "Tasks"
            };

            var itemId = await SendAsync(command);

            var item = await FindAsync<Domain.Entities.Task>(itemId);

            item.Should().NotBeNull();
            item.EmployeeId.Should().Be(command.EmployeeId);
            item.Title.Should().Be(command.Title);
            item.CreatedBy.Should().Be(userId);
            item.Created.Should().BeCloseTo(DateTime.Now, 10000);
            item.LastModifiedBy.Should().BeNull();
            item.LastModified.Should().BeNull();
        }
    }
}
