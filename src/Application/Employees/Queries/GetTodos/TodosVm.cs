using System.Collections.Generic;

namespace EmployeeTask.Application.Employees.Queries.GetTodos
{
    public class TodosVm
    {
        public IList<PriorityLevelDto> PriorityLevels { get; set; }

        public IList<TaskStateDto> TaskStates { get; set; }

        public IList<EmployeeDto> Employees { get; set; }
    }
}
