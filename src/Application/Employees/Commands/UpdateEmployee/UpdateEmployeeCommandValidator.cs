using EmployeeTask.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateEmployeeCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
                .MustAsync(BeUniqueTitle).WithMessage("The specified name already exists.");
        }

        public async Task<bool> BeUniqueTitle(UpdateEmployeeCommand model, string name, CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Where(l => l.Id != model.Id)
                .AllAsync(l => l.Name != name);
        }
    }
}
