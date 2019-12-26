using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Practice_Mvc.Models;

namespace Practice_Mvc.Infrastructure
{
    public interface ICurrentUser
    {
        ApplicationUser User { get; }
    }
}
