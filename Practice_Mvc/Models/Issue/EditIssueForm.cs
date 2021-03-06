﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Practice_Mvc.Domain;
using Practice_Mvc.Infrastructure.Mapping;

namespace Practice_Mvc.Models.Issue
{
    public class EditIssueForm: IMapFrom<Domain.Issue>
    {
        public int IssueID { get; set; }
        public string Subject { get; set; }
        public string CreatorUserName { get; set; }
        public string Body { get; set; }

        [Display(Name = "Issue IssueType")]
        public IssueType IssueType { get; set; }
        public SelectListItem[] AvailableIssueTypes { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedToId { get; set; }
        public SelectListItem[] AvailableUsers { get; set; }
    }
}