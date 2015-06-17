namespace BlogSystem.Web.Infrastructure.Registries
{
    using System.Security.Principal;
    using System.Web;

    using BlogSystem.Web.Infrastructure.Cache;

    using StructureMap.Configuration.DSL;

    public class MvcRegistry : Registry
    {
        public MvcRegistry()
        {
            this.For<IIdentity>().Use(() => HttpContext.Current.User.Identity);

            this.For<ICacheService>().Use<HttpRuntimeCacheService>();
        }
    }
}
