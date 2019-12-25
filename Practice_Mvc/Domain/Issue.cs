using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Practice_Mvc.Models;

namespace Practice_Mvc.Domain
{
    public class Issue
    {
        public int IssueID { get; set; }

        public ApplicationUser Creator { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        //For EF...
        protected Issue()
        {

        }

        public Issue(ApplicationUser creator, string subject, string body)
        {
            Creator = creator;
            Subject = subject;
            Body = body;
            CreatedAt = DateTime.Now;
        }
    }
}