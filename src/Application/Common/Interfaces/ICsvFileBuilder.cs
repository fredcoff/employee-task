using EmployeeTask.Application.Employees.Queries.ExportTodos;
using System.Collections.Generic;

namespace EmployeeTask.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTasksFile(IEnumerable<TaskRecord> records);
    }
}
