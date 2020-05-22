using EmployeeTask.Application.Common.Mappings;
using EmployeeTask.Domain.Entities;
using System.Collections.Generic;

namespace EmployeeTask.Application.Employees.Queries.GetTodos
{
    public class EmployeeDto : IMapFrom<Employee>
    {
        public EmployeeDto()
        {
            Tasks = new List<TaskDto>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<TaskDto> Tasks { get; set; }
    }
}
