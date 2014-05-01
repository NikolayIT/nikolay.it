using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogSystem.Web.Startup))]
namespace BlogSystem.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
