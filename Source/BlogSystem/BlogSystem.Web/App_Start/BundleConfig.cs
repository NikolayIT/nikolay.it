namespace BlogSystem.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);

            RegisterStyles(bundles);
            
            BundleTable.EnableOptimizations = true;
        }

        public static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));
        }

        public static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/font-awesome.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.themes.unify.app.css",
                      "~/Content/bootstrap.themes.unify.css",
                      "~/Content/site.blog.css",
                      "~/Content/bootstrap.themes.unify.red.css",
                      "~/Content/site.common.css"));
        }
    }
}
