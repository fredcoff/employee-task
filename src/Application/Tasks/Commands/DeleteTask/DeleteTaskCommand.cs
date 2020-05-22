using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Common.Interfaces;
using EmployeeTask.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Tasks.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Task), request.Id);
            }

            _context.Tasks.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
