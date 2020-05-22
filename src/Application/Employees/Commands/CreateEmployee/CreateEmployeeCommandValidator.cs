using EmployeeTask.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeTask.Application.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateEmployeeCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
                .MustAsync(BeUniqueTitle).WithMessage("The specified name already exists.");
        }

        public async Task<bool> BeUniqueTitle(string name, CancellationToken cancellationToken)
        {
            return await _context.Employees
                .AllAsync(l => l.Name != name);
        }
    }
}
