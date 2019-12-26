using Practice_Mvc.Data;
using Practice_Mvc.Domain;
using Practice_Mvc.Filters;
using Practice_Mvc.Models.Issue;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Practice_Mvc.Infrastructure;

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

		private SelectListItem[] GetAvailableUsers()
		{
			return _context.Users.Select(u => new SelectListItem { Text = u.UserName, Value = u.Id }).ToArray();
		}

		private SelectListItem[] GetAvailableIssueTypes()
		{
			return Enum.GetValues(typeof(IssueType))
				.Cast<IssueType>()
				.Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
				.ToArray();
		}

		[ChildActionOnly]
		public ActionResult YourIssuesWidget()
		{
			var models = _context.Issues.Where(i => i.AssignedTo.Id == _currentUser.User.Id)
                .Select(i => new IssueSummaryViewModel
                {
                    IssueID = i.IssueID,
                    Subject = i.Subject,
                    Type = i.IssueType,
                    CreatedAt = i.CreatedAt,
                    Creator = i.Creator.UserName
                }).ToArray();

			return PartialView(models);
		}

		[ChildActionOnly]
		public ActionResult CreatedByYouWidget()
		{
			var models = from i in _context.Issues
						 where i.Creator.Id == _currentUser.User.Id
						 select new IssueSummaryViewModel
						 {
							 IssueID = i.IssueID,
							 Subject = i.Subject,
							 Type = i.IssueType,
							 CreatedAt = i.CreatedAt,
							 AssignedTo = i.AssignedTo.UserName
						 };

			return PartialView(models.ToArray());
		}

		[ChildActionOnly]
		public ActionResult AssignmentStatsWidget()
		{
			var stats = _context.Users.Select(u => new AssignmentStatsViewModel
			{
				UserName = u.UserName,
				Enhancements = u.Assignments.Count(i => i.IssueType == IssueType.Enhancement),
				Bugs = u.Assignments.Count(i => i.IssueType == IssueType.Bug),
				Support = u.Assignments.Count(i => i.IssueType == IssueType.Support),
				Other = u.Assignments.Count(i => i.IssueType == IssueType.Other),
			}).ToArray();

			return PartialView(stats);
		}

		public ActionResult New()
		{
			var form = new NewIssueForm
			{
				AvailableUsers = GetAvailableUsers(),
				AvailableIssueTypes = GetAvailableIssueTypes()
			};
			return View(form);
		}

		[HttpPost, ValidateAntiForgeryToken, Log("Created issue")]
		public ActionResult New(NewIssueForm form)
		{
			if (!ModelState.IsValid)
			{
				form.AvailableUsers = GetAvailableUsers();
				form.AvailableIssueTypes = GetAvailableIssueTypes();
				return View(form);
			}

			var assignedToUser = _context.Users.Single(u => u.Id == form.AssignedToUserID);

			_context.Issues.Add(new Issue(_currentUser.User, assignedToUser, form.IssueType, form.Subject, form.Body));

			_context.SaveChanges();

			return RedirectToAction("Index", "Home");
		}

		[Log("Viewed issue {id}")]
		public ActionResult View(int id)
		{
			var issue = _context.Issues
				.Include(i => i.AssignedTo)
				.Include(i => i.Creator)
				.SingleOrDefault(i => i.IssueID == id);

			if (issue == null)
			{
				throw new ApplicationException("Issue not found!");
			}

			return View(new IssueDetailsViewModel
			{
				IssueID = issue.IssueID,
				Subject = issue.Subject,
				CreatedAt = issue.CreatedAt,
				AssignedTo = issue.AssignedTo.UserName,
				Creator = issue.Creator.UserName,
				IssueType = issue.IssueType,
				Body = issue.Body
			});
		}

		[Log("Started to edit issue {id}")]
		public ActionResult Edit(int id)
		{
			var issue = _context.Issues
				.Include(i => i.AssignedTo)
				.Include(i => i.Creator)
				.SingleOrDefault(i => i.IssueID == id);

			if (issue == null)
			{
				throw new ApplicationException("Issue not found!");
			}

			return View(new EditIssueForm
			{
				IssueID = issue.IssueID,
				Subject = issue.Subject,
				AssignedToUserID = issue.AssignedTo.Id,
				AvailableUsers = GetAvailableUsers(),
				Creator = issue.Creator.UserName,
				IssueType = issue.IssueType,
				AvailableIssueTypes = GetAvailableIssueTypes(),
				Body = issue.Body
			});
		}

		[HttpPost, Log("Saving changes")]
		public ActionResult Edit(EditIssueForm form)
		{
			if (!ModelState.IsValid)
			{
				form.AvailableUsers = GetAvailableUsers();
				form.AvailableIssueTypes = GetAvailableIssueTypes();
				return View(form);
			}

			var issue = _context.Issues.SingleOrDefault(i => i.IssueID == form.IssueID);

			if (issue == null)
			{
				throw new ApplicationException("Issue not found!");
			}

			var assignedToUser = _context.Users.Single(u => u.Id == form.AssignedToUserID);

			issue.Subject = form.Subject;
			issue.AssignedTo = assignedToUser;
			issue.Body = form.Body;
			issue.IssueType = form.IssueType;


			return RedirectToAction("View", new { id = form.IssueID });
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