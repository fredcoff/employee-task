using EmployeeTask.Domain.Entities;
using EmployeeTask.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using EmployeeTask.Domain.Enums;
using System;

namespace EmployeeTask.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async System.Threading.Tasks.Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
            }
        }

        public static async System.Threading.Tasks.Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.Employees.Any())
            {
                context.Employees.Add(new Employee
                {
                    Name = "John Smith",
                    Tasks =
                    {
                        new Domain.Entities.Task {
                            Title = "Requirements", State = TaskState.Closed, Priority = PriorityLevel.Critical,
                            Estimate = new DateTime(2020, 05, 20)
                        },
                        new Domain.Entities.Task {
                            Title = "UI/UX Design", State = TaskState.Resolved, Priority = PriorityLevel.Low,
                            Estimate = new DateTime(2020, 05, 21)
                        },
                        new Domain.Entities.Task {
                            Title = "Making Prototypes", State = TaskState.New, Priority = PriorityLevel.Medium,
                            Estimate = new DateTime(2020, 05, 22)
                        },
                        new Domain.Entities.Task {
                            Title = "Milestone 1", State = TaskState.Active, Priority = PriorityLevel.Critical,
                            Estimate = new DateTime(2020, 05, 22)
                        },
                        new Domain.Entities.Task {
                            Title = "Unit Testing", State = TaskState.New, Priority = PriorityLevel.Low,
                            Estimate = new DateTime(2020, 05, 22)
                        },
                        new Domain.Entities.Task {
                            Title = "Quality Assurance", State = TaskState.New, Priority = PriorityLevel.Low,
                            Estimate = new DateTime(2020, 05, 27)
                        },
                        new Domain.Entities.Task {
                            Title = "Demo", State = TaskState.New, Priority = PriorityLevel.Low,
                            Estimate = new DateTime(2020, 05, 27)
                        },
                        new Domain.Entities.Task {
                            Title = "Milestone 2", State = TaskState.New, Priority = PriorityLevel.Low,
                            Estimate = new DateTime(2020, 05, 27)
                        }
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
