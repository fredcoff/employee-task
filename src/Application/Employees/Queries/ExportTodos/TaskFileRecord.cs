using EmployeeTask.Application.Common.Mappings;
using EmployeeTask.Domain.Entities;

namespace EmployeeTask.Application.Employees.Queries.ExportTodos
{
    public class TaskRecord : IMapFrom<Task>
    {
        public string Title { get; set; }

        public int State { get; set; }
    }
}
