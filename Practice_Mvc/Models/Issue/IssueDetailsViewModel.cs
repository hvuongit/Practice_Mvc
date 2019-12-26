using System;
using Practice_Mvc.Domain;
using Practice_Mvc.Infrastructure.Mapping;

namespace Practice_Mvc.Models.Issue
{
    public class IssueDetailsViewModel : IMapFrom<Domain.Issue>
    {
		public int IssueID { get; set; }
        public string Subject { get; set; }
        public string CreatorUserName { get; set; }
        public string AssignedToId { get; set; }
        public DateTime CreatedAt { get; set; }
        public IssueType IssueType { get; set; }
        public string Body { get; set; }
	}
}