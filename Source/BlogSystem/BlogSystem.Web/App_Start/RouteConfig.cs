namespace BlogSystem.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Blog post",
                url: "Blog/{year}/{month}/{title}/{id}",
                defaults: new { controller = "Blog", action = "Post" });

            routes.MapRoute(
                name: "Page",
                url: "Pages/{permalink}",
                defaults: new { controller = "Pages", action = "Page" });

            routes.MapRoute(
                name: "Old blog links",
                url: "{year}-{month}/{title}-{id}.html",
                defaults: new { controller = "Redirects", action = "OldSystemBlogPost" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
