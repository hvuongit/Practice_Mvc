using System.Linq;
using AutoMapper;
using Practice_Mvc.Domain;
using Practice_Mvc.Infrastructure.Tasks;
using Practice_Mvc.Models;
using Practice_Mvc.Models.Issue;

namespace Practice_Mvc
{
    public class AutoMapperConfig : IRunAtInit
    {
        public void Execute()
        {
            Mapper.CreateMap<Domain.Issue, IssueSummaryViewModel>();

            Mapper.CreateMap<Domain.Issue, IssueDetailsViewModel>();

            Mapper.CreateMap<Domain.Issue, EditIssueForm>();

            Mapper.CreateMap<ApplicationUser, AssignmentStatsViewModel>()
                .ForMember(m => m.Enhancements, opt =>
                    opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Enhancement)))
                .ForMember(m => m.Bugs, opt =>
                    opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Bug)))
                .ForMember(m => m.Support, opt =>
                    opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Support)))
                .ForMember(m => m.Other, opt =>
                    opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Other)));

        }
    }
}