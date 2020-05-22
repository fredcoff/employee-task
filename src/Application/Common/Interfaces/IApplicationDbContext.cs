using EmployeeTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Employee> Employees { get; set; }

        DbSet<Domain.Entities.Task> Tasks { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
