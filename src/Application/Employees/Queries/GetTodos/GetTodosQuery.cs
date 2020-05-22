using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeTask.Application.Common.Interfaces;
using EmployeeTask.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Employees.Queries.GetTodos
{
    public class GetTodosQuery : IRequest<TodosVm>
    {
    }

    public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, TodosVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodosVm> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            var Employees = await _context.Employees
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);

            foreach (EmployeeDto ed in Employees)
            {
                ed.Tasks = ed.Tasks.OrderBy(t => t.Priority).ToList();
            }

            return new TodosVm
            {
                PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                    .Cast<PriorityLevel>()
                    .Select(p => new PriorityLevelDto { Value = (int)p, Name = p.ToString() })
                    .ToList(),

                TaskStates = Enum.GetValues(typeof(TaskState))
                    .Cast<TaskState>()
                    .Select(p => new TaskStateDto { Value = (int)p, Name = p.ToString() })
                    .ToList(),

                Employees = Employees
            };
        }
    }
}
