using Practice_Mvc.Models;

namespace Practice_Mvc.Infrastructure
{
    public interface ICurrentUser
    {
        ApplicationUser User { get; }
    }
}