using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Common.Interfaces;
using EmployeeTask.Domain.Entities;
using EmployeeTask.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Tasks.Commands.UpdateTaskDetail
{
    public class UpdateTaskDetailCommand : IRequest
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public PriorityLevel Priority { get; set; }

        public string Description { get; set; }
    }

    public class UpdateTaskDetailCommandHandler : IRequestHandler<UpdateTaskDetailCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTaskDetailCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTaskDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Tasks.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Task), request.Id);
            }

            entity.EmployeeId = request.EmployeeId;
            entity.Priority = request.Priority;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
