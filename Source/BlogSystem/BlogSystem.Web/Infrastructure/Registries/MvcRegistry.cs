namespace BlogSystem.Web.Infrastructure.Registries
{
    using System.Security.Principal;
    using System.Web;

    using StructureMap.Configuration.DSL;

    public class MvcRegistry : Registry
    {
        public MvcRegistry()
        {
            For<IIdentity>().Use(() => HttpContext.Current.User.Identity);
        }
    }
}
