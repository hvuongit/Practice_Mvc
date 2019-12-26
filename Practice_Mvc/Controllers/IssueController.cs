using Practice_Mvc.Data;
using Practice_Mvc.Domain;
using Practice_Mvc.Filters;
using Practice_Mvc.Models.Issue;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
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
			var models = _context.Issues
                .Where(i => i.AssignedTo.Id == _currentUser.User.Id)
                .Project().To<IssueSummaryViewModel>();

			return PartialView(models.ToArray());
		}

		[ChildActionOnly]
		public ActionResult CreatedByYouWidget()
        {
            var models = _context.Issues
                .Where(i => i.Creator.Id == _currentUser.User.Id)
                .Project().To<IssueSummaryViewModel>();
               
			return PartialView(models.ToArray());
		}

		[ChildActionOnly]
		public ActionResult AssignmentStatsWidget()
        {
            var stats = _context.Users.Project().To<AssignmentStatsViewModel>();

			return PartialView(stats.ToArray());
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
			var model = _context.Issues
                .Project().To<IssueDetailsViewModel>()
				.SingleOrDefault(i => i.IssueID == id);

			if (model == null)
			{
				throw new ApplicationException("Issue not found!");
			}

			return View(model);
		}

		[Log("Started to edit issue {id}")]
		public ActionResult Edit(int id)
		{
			var model = _context.Issues
                .Project().To<EditIssueForm>()
				.SingleOrDefault(i => i.IssueID == id);

			if (model == null)
			{
				throw new ApplicationException("Issue not found!");
			}

			return View(model);
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