namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;

    using StructureMap;

    public class BaseController : Controller
    {
        protected readonly IContainer Container;
        protected readonly IRepository<Setting> Settings;

        public BaseController()
        {
            this.Container = (IContainer)System.Web.HttpContext.Current.Items["_Container"];
            this.Settings = this.Container.GetInstance<IRepository<Setting>>();
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            return base.BeginExecute(requestContext, callback, state);
        }
    }
}