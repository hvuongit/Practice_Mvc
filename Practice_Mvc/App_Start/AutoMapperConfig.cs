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
            Mapper.CreateMap<Domain.Issue, IssueSummaryViewModel>()
                .ForMember(m => m.Creator,
                    opt => opt.MapFrom(i => i.Creator.UserName))
                .ForMember(m => m.AssignedTo,
                    opt => opt.MapFrom(i => i.AssignedTo.UserName))
                .ForMember(m => m.Type,
                    opt => opt.MapFrom(i => i.IssueType));

            Mapper.CreateMap<Domain.Issue, IssueDetailsViewModel>()
                .ForMember(m => m.AssignedTo,
                    opt => opt.MapFrom(i => i.AssignedTo.Id))
                .ForMember(m => m.Creator,
                opt => opt.MapFrom(i => i.Creator.UserName));

            Mapper.CreateMap<Domain.Issue, EditIssueForm>()
                .ForMember(m => m.AssignedToUserID,
                    opt => opt.MapFrom(i => i.AssignedTo.Id))
                .ForMember(m => m.Creator,
                    opt => opt.MapFrom(i => i.Creator.UserName));

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