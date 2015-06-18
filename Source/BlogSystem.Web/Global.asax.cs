namespace BlogSystem.Web
{
    using System;
    using System.Data.Entity;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using BlogSystem.Data;
    using BlogSystem.Data.Migrations;
    using BlogSystem.Web.Infrastructure;
    using BlogSystem.Web.Infrastructure.Mapping;

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

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(() => this.Container ?? Infrastructure.ObjectFactory.Container));
            
            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute();
        }

        protected void Application_BeginRequest()
        {
            this.Container = Infrastructure.ObjectFactory.Container.GetNestedContainer();

            // Remove www. from URL
            if (this.Request.Url.Host.StartsWith("www.") && !this.Request.Url.IsLoopback)
            {
                var builder = new UriBuilder(this.Request.Url)
                                  {
                                      Host = this.Request.Url.Host.Replace("www.", string.Empty)
                                  };
                this.Response.RedirectPermanent(builder.ToString(), true);
            }
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
