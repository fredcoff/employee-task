using EmployeeTask.Application.Common.Exceptions;
using EmployeeTask.Application.Common.Interfaces;
using EmployeeTask.Domain.Entities;
using EmployeeTask.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Tasks.Commands.UpdateTask
{
    public partial class UpdateTaskCommand : IRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public TaskState State { get; set; }
    }

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Tasks.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Task), request.Id);
            }

            entity.Title = request.Title;
            entity.State = request.State;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
