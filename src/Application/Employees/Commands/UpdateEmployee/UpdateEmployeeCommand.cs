using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Common.Interfaces;
using EmployeeTask.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateEmployeeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Employees.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Employee), request.Id);
            }

            entity.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
