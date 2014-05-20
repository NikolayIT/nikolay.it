namespace BlogSystem.Web
{
    using System.Data.Entity;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using BlogSystem.Data;
    using BlogSystem.Data.Migrations;
    using BlogSystem.Web.Infrastructure;
    using BlogSystem.Web.Infrastructure.Mapping;
    using BlogSystem.Web.Infrastructure.Registries;

    using StructureMap;

    public class MvcApplication : HttpApplication
    {
        public IContainer Container
        {
            get
            {
                return (IContainer)HttpContext.Current.Items["_Container"];
            }

            set
            {
                HttpContext.Current.Items["_Container"] = value;
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, DefaultMigrationConfiguration>());

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(() => this.Container ?? ObjectFactory.Container));

            ObjectFactory.Configure(cfg =>
            {
                cfg.AddRegistry(new StandardRegistry());
                cfg.AddRegistry(new ControllerRegistry());
                //// cfg.AddRegistry(new ActionFilterRegistry(() => Container ?? ObjectFactory.Container));
                cfg.AddRegistry(new MvcRegistry());
                //// TODO: cfg.AddRegistry(new TaskRegistry());
                cfg.AddRegistry(new DataRegistry());
            });

            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute();
        }

        protected void Application_BeginRequest()
        {
            this.Container = ObjectFactory.Container.GetNestedContainer();
        }

        protected void Application_EndRequest()
        {
            try
            {
                // Code to be executed on end request
            }
            finally
            {
                this.Container.Dispose();
                this.Container = null;
            }
        }
    }
}
