using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Practice_Mvc.Data;
using Practice_Mvc.Domain;
using Practice_Mvc.Filters;
using Practice_Mvc.Infrastructure;
using Practice_Mvc.Models.Issue;

namespace Practice_Mvc.Controllers
{
    public class IssueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public IssueController(ApplicationDbContext context,
            ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        [ChildActionOnly]
        public ActionResult IssueWidget()
        {
            var models = from i in _context.Issues
                select new IssueSummaryViewModel
                {
                    IssueID = i.IssueID,
                    Subject = i.Subject,
                    CreatedAt = i.CreatedAt
                };

            return PartialView(models.ToArray());
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Log("Created issue")]
        public ActionResult New(NewIssueForm form)
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Find(userId);
            
            _context.Issues.Add(new Issue(user, form.Subject, form.Body));

            _context.Logs.Add(new LogAction(user, "New", "Issue", "Create issue"));

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Log("Viewed issue {id}")]
        public ActionResult View(int id)
        {
            var issue = _context.Issues.Find(id);

            if (issue == null)
            {
                throw new ApplicationException("Issue not found!");
            }

            var userId = User.Identity.GetUserId();
            var user = _context.Users.Find(userId);

            _context.Logs.Add(new LogAction(user, "View", "Issue", "Viewed issue" + id));

            return View(new IssueDetailsViewModel
            {
                IssueID = issue.IssueID,
                Subject = issue.Subject,
                CreatedAt = issue.CreatedAt,
                Body = issue.Body
            });
        }

        [HttpPost, ValidateAntiForgeryToken, Log("Deleted issue {id}")]
        public ActionResult Delete(int id)
        {
            var issue = _context.Issues.Find(id);

            if (issue == null)
            {
                throw new ApplicationException("Issue not found!");
            }

            _context.Issues.Remove(issue);

            var userId = User.Identity.GetUserId();
            var user = _context.Users.Find(userId);

            _context.Logs.Add(new LogAction(user, "Delete", "Issue", "Deleted issue" + id));

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}