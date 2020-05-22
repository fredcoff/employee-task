using AutoMapper;
using EmployeeTask.Application.Common.Mappings;
using EmployeeTask.Domain.Entities;
using System;

namespace EmployeeTask.Application.Employees.Queries.GetTodos
{
    public class TaskDto : IMapFrom<Task>
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string Title { get; set; }

        public int State { get; set; }

        public int Priority { get; set; }

        public string Description { get; set; }

        public string Estimate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Task, TaskDto>()
                .ForMember(d => d.Estimate, opt => opt.MapFrom(s => s.Estimate != null ? ((DateTime)s.Estimate).ToString("MMMM dd") : ""))
                .ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority))
                .ForMember(d => d.State, opt => opt.MapFrom(s => (int)s.State));
        }
    }
}
