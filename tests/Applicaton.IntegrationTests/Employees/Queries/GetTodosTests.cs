using EmployeeTask.Application.Employees.Queries.GetTodos;
using EmployeeTask.Domain.Entities;
using EmployeeTask.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTask.Application.IntegrationTests.Employees.Queries
{
    using static Testing;

    public class GetTodosTests : TestBase
    {
        [Test]
        public async System.Threading.Tasks.Task ShouldReturnPriorityLevels()
        {
            var query = new GetTodosQuery();

            var result = await SendAsync(query);

            result.PriorityLevels.Should().NotBeEmpty();
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldReturnTaskStates()
        {
            var query = new GetTodosQuery();

            var result = await SendAsync(query);

            result.TaskStates.Should().NotBeEmpty();
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldReturnAllEmployeesAndTasks()
        {
            await AddAsync(new Employee
            {
                Name = "John Smith",
                Tasks =
                {
                    new Domain.Entities.Task { Title = "Requirements", State = TaskState.Closed, Priority = PriorityLevel.Critical },
                    new Domain.Entities.Task { Title = "UI/UX Design", State = TaskState.Resolved, Priority = PriorityLevel.Low },
                    new Domain.Entities.Task { Title = "Making Prototypes", State = TaskState.New, Priority = PriorityLevel.Medium},
                    new Domain.Entities.Task { Title = "Milestone 1", State = TaskState.Active, Priority = PriorityLevel.Critical },
                    new Domain.Entities.Task { Title = "Unit Testing", State = TaskState.New, Priority = PriorityLevel.Low },
                    new Domain.Entities.Task { Title = "Quality Assurance", State = TaskState.New, Priority = PriorityLevel.Low },
                    new Domain.Entities.Task { Title = "Demo", State = TaskState.New, Priority = PriorityLevel.Low },
                    new Domain.Entities.Task { Title = "Milestone 2", State = TaskState.New, Priority = PriorityLevel.Low }
                }
            });

            var query = new GetTodosQuery();

            var result = await SendAsync(query);

            result.Employees.Should().HaveCount(1);
            result.Employees.First().Tasks.Should().HaveCount(8);
        }
    }
}
