using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Employees.Commands.CreateEmployee;
using EmployeeTask.Application.Employees.Commands.DeleteEmployee;
using EmployeeTask.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EmployeeTask.Application.IntegrationTests.Employees.Commands
{
    using static Testing;

    public class DeleteEmployeeTests : TestBase
    {
        [Test]
        public void ShouldRequireValidEmployeeId()
        {
            var command = new DeleteEmployeeCommand { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldDeleteEmployee()
        {
            var employeeId = await SendAsync(new CreateEmployeeCommand
            {
                Name = "John Smith"
            });

            await SendAsync(new DeleteEmployeeCommand 
            { 
                Id = employeeId 
            });

            var list = await FindAsync<Employee>(employeeId);

            list.Should().BeNull();
        }
    }
}
