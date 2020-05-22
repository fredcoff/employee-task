using EmployeeTask.Application.Employees.Queries.ExportTodos;
using CsvHelper.Configuration;
using System.Globalization;
using EmployeeTask.Domain.Enums;

namespace EmployeeTask.Infrastructure.Files.Maps
{
    public class TaskRecordMap : ClassMap<TaskRecord>
    {
        public TaskRecordMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.State).ConvertUsing(c => c.State == (int)TaskState.Closed ? "Yes" : "No");
        }
    }
}
