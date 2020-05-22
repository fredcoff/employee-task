using System;
using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Employees.Commands.CreateEmployee;
using EmployeeTask.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace EmployeeTask.Application.IntegrationTests.Employees.Commands
{
    using static Testing;

    public class CreateEmployeeTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateEmployeeCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldRequireUniqueName()
        {
            await SendAsync(new CreateEmployeeCommand
            {
                Name = "John Smith"
            });

            var command = new CreateEmployeeCommand
            {
                Name = "John Smith"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldCreateEmployee()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new CreateEmployeeCommand
            {
                Name = "Jane"
            };

            var id = await SendAsync(command);

            var list = await FindAsync<Employee>(id);

            list.Should().NotBeNull();
            list.Name.Should().Be(command.Name);
            list.CreatedBy.Should().Be(userId);
            list.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}
