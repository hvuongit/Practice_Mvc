using System.Security.Principal;
using Practice_Mvc.Data;

namespace Practice_Mvc.Infrastructure
{
    public class CurrentUser
    {
        private readonly IIdentity _identity;
        private readonly ApplicationDbContext _context;

        public CurrentUser(IIdentity identity, ApplicationDbContext context)
        {
            _identity = identity;
            _context = context;
        }
    }
}