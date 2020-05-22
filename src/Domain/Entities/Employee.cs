using EmployeeTask.Domain.Common;
using System.Collections.Generic;

namespace EmployeeTask.Domain.Entities
{
    public class Employee : AuditableEntity
    {
        public Employee()
        {
            Tasks = new List<Task>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Colour { get; set; }

        public IList<Task> Tasks { get; set; }
    }
}
