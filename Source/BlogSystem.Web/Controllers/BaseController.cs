namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure;
    using BlogSystem.Web.Infrastructure.ActionResults;
    using BlogSystem.Web.Infrastructure.Cache;

    using StructureMap;

    public abstract class BaseController : Controller
    {
        protected readonly IContainer Container;
        protected readonly IRepository<Setting> Settings;
        protected readonly ICacheService Cache;

        protected BaseController()
        {
            this.Container = (IContainer)System.Web.HttpContext.Current.Items["_Container"];
            this.Settings = this.Container.GetInstance<IRepository<Setting>>();
            this.Cache = this.Container.GetInstance<ICacheService>();
        }

        protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action)
            where TController : Controller
        {
            var method = action.Body as MethodCallExpression;
            if (method == null)
            {
                throw new ArgumentException("Expected method call");
            }

            return this.RedirectToAction(method.Method.Name);
        }

        protected StandardJsonResult JsonError(string errorMessage)
        {
            var result = new StandardJsonResult();

            result.AddError(errorMessage);

            return result;
        }

        protected StandardJsonResult<T> JsonSuccess<T>(T data)
        {
            return new StandardJsonResult<T> { Data = data };
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.ViewBag.Settings = new SettingsManager(this.GetSettings);

            return base.BeginExecute(requestContext, callback, state);
        }

        protected IDictionary<string, string> GetSettings()
        {
            return this.Settings.All().ToDictionary(x => x.Name, x => x.Value);
        }
    }
}