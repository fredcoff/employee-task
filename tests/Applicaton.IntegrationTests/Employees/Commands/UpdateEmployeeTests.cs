using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Employees.Commands.CreateEmployee;
using EmployeeTask.Application.Employees.Commands.UpdateEmployee;
using EmployeeTask.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EmployeeTask.Application.IntegrationTests.Employees.Commands
{
    using static Testing;

    public class UpdateEmployeeTests : TestBase
    {
        [Test]
        public void ShouldRequireValidEmployeeId()
        {
            var command = new UpdateEmployeeCommand
            {
                Id = 99,
                Name = "John Smith"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldRequireUniqueName()
        {
            var employeeId = await SendAsync(new CreateEmployeeCommand
            {
                Name = "John Smith"
            });

            await SendAsync(new CreateEmployeeCommand
            {
                Name = "Jane"
            });

            var command = new UpdateEmployeeCommand
            {
                Id = employeeId,
                Name = "Jane"
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name"))
                    .And.Errors["Name"].Should().Contain("The specified name already exists.");
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldUpdateEmployee()
        {
            var userId = await RunAsDefaultUserAsync();

            var employeeId = await SendAsync(new CreateEmployeeCommand
            {
                Name = "John Smith"
            });

            var command = new UpdateEmployeeCommand
            {
                Id = employeeId,
                Name = "John Becker"
            };

            await SendAsync(command);

            var list = await FindAsync<Employee>(employeeId);

            list.Name.Should().Be(command.Name);
            list.LastModifiedBy.Should().NotBeNull();
            list.LastModifiedBy.Should().Be(userId);
            list.LastModified.Should().NotBeNull();
            list.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}
