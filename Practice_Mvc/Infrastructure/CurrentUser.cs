using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Practice_Mvc.Data;
using Practice_Mvc.Models;

namespace Practice_Mvc.Infrastructure
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IIdentity _identity;
        private readonly ApplicationDbContext _context;

        private ApplicationUser _user;

        public CurrentUser(IIdentity identity, ApplicationDbContext context)
        {
            _identity = identity;
            _context = context;
        }
        public ApplicationUser User => _user ?? (_user = _context.Users.Find(_identity.GetUserId()) ); 
    }
}  