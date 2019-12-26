using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Practice_Mvc.Domain;
using ApplicationUser = Practice_Mvc.Models.ApplicationUser;

namespace Practice_Mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Issue> Issues { get; set; }
        public DbSet<LogAction> Logs { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}