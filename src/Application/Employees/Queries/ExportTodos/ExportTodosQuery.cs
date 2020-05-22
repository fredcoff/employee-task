using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeTask.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Employees.Queries.ExportTodos
{
    public class ExportTodosQuery : IRequest<ExportTodosVm>
    {
        public int EmployeeId { get; set; }
    }

    public class ExportTodosQueryHandler : IRequestHandler<ExportTodosQuery, ExportTodosVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICsvFileBuilder _fileBuilder;

        public ExportTodosQueryHandler(IApplicationDbContext context, IMapper mapper, ICsvFileBuilder fileBuilder)
        {
            _context = context;
            _mapper = mapper;
            _fileBuilder = fileBuilder;
        }

        public async Task<ExportTodosVm> Handle(ExportTodosQuery request, CancellationToken cancellationToken)
        {
            var vm = new ExportTodosVm();

            var records = await _context.Tasks
                    .Where(t => t.EmployeeId == request.EmployeeId)
                    .ProjectTo<TaskRecord>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            vm.Content = _fileBuilder.BuildTasksFile(records);
            vm.ContentType = "text/csv";
            vm.FileName = "Tasks.csv";

            return await Task.FromResult(vm);
        }
    }
}
