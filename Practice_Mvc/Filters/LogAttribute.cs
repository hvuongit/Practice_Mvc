using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Practice_Mvc.Data;
using Practice_Mvc.Domain;

namespace Practice_Mvc.Filters
{
    public class LogAttribute: ActionFilterAttribute
    {
        private IDictionary<string, object> _parameters;
        public ApplicationDbContext Context { get; set; }
        public string Description { get; set; }

        public LogAttribute(string description)
        {
            Description = description;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _parameters = filterContext.ActionParameters;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var userId = filterContext.HttpContext.User.Identity.GetUserId();

            var user = Context.Users.Find(userId);

            foreach (var kvp in _parameters)
            {
                Description = Description.Replace("{" + kvp.Key + "}", kvp.ToString());
            }

            Context.Logs.Add(new LogAction(user, filterContext.ActionDescriptor.ActionName , 
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, Description));
            Context.SaveChanges();
        }
    }
}