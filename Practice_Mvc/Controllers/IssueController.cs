using Microsoft.AspNet.Identity;
using Practice_Mvc.Data;
using Practice_Mvc.Domain;
using Practice_Mvc.Filters;
using Practice_Mvc.Models.Issue;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Practice_Mvc.Controllers
{
    public class IssueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IssueController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Issue
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
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}