using System;
using Practice_Mvc.Domain;

namespace Practice_Mvc.Models.Issue
{
    public class IssueDetailsViewModel
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