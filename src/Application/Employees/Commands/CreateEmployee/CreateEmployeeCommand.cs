using EmployeeTask.Application.Common.Interfaces;
using EmployeeTask.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Employees.Commands.CreateEmployee
{
    public partial class CreateEmployeeCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateEmployeeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = new Employee();

            entity.Name = request.Name;

            _context.Employees.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
