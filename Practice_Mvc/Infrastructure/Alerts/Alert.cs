using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice_Mvc.Infrastructure.Alerts
{
    public class Alert
    {
        public string AlertClass { get; set; }
        public string Message { get; set; }

        public Alert(string alertClass, string message)
        {
            this.AlertClass = alertClass;
            this.Message = message;
        }
    }
}