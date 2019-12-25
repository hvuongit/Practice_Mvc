using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Practice_Mvc.Startup))]
namespace Practice_Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
