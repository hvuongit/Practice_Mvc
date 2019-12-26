using System;
using Practice_Mvc.Domain;

namespace Practice_Mvc.Models.Issue
{
    public class IssueSummaryViewModel
    {
		public int IssueID { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Creator { get; set; }
        public IssueType Type { get; set; }
        public string AssignedTo { get; set; }
    }
}
