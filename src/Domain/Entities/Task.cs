using EmployeeTask.Domain.Common;
using EmployeeTask.Domain.Enums;
using System;

namespace EmployeeTask.Domain.Entities
{
    public class Task : AuditableEntity
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TaskState State { get; set; }

        public DateTime? Estimate { get; set; }

        public PriorityLevel Priority { get; set; }


        public Employee List { get; set; }
    }
}
