using EmployeeTask.Application.Common.Interfaces;
using System;

namespace EmployeeTask.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
