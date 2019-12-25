using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practice_Mvc.Data;
using Practice_Mvc.Domain;
using Practice_Mvc.Infrastructure;

namespace Practice_Mvc.Filters
{
    public class LogAttribute: ActionFilterAttribute
    {
        private IDictionary<string, object> _parameters;
        public ApplicationDbContext Context { get; set; }
        public ICurrentUser CurrentUser { get; set; }

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
            var description = Description;

            foreach (var kvp in _parameters)
            {
                description = description.Replace("{" + kvp.Key + "}", kvp.Value.ToString());
            }

            Context.Logs.Add(new LogAction(CurrentUser.User, filterContext.ActionDescriptor.ActionName,
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, description));

            Context.SaveChanges();
        }
    }
}