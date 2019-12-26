using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Practice_Mvc.Domain;

namespace Practice_Mvc.Models.Issue
{
    public class EditIssueForm
    {
        public int IssueID { get; set; }
        public string Subject { get; set; }
        public string Creator { get; set; }
        public string Body { get; set; }

        [Display(Name = "Issue Type")]
        public IssueType IssueType { get; set; }
        public SelectListItem[] AvailableIssueTypes { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedToUserID { get; set; }
        public SelectListItem[] AvailableUsers { get; set; }
    }
}