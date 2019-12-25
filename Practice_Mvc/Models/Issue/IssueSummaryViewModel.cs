using System;

namespace Practice_Mvc.Models.Issue
{
    public class IssueSummaryViewModel
    {
        public int IssueID { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}