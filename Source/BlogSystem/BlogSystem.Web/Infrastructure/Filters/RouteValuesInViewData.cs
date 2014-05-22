namespace BlogSystem.Web.Infrastructure.Filters
{
    using System.Web.Mvc;

    public class PassRouteValuesToViewDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routes = filterContext.RouteData.Values;
            var viewData = filterContext.Controller.ViewData;
            foreach (var route in routes)
            {
                viewData.Add(route.Key, route.Value);
            }
        }
    }
}