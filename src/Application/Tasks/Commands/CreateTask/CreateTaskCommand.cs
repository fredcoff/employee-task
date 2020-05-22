using EmployeeTask.Application.Common.Interfaces;
using EmployeeTask.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using EmployeeTask.Domain.Enums;

namespace EmployeeTask.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<int>
    {
        public int EmployeeId { get; set; }

        public string Title { get; set; }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Task
            {
                EmployeeId = request.EmployeeId,
                Title = request.Title,
                State = TaskState.New,
                Priority = PriorityLevel.Critical
            };

            _context.Tasks.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
