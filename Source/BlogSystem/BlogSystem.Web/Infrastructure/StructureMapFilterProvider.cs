namespace BlogSystem.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using StructureMap;

    public class StructureMapFilterProvider : FilterAttributeFilterProvider
    {
        private readonly Func<IContainer> currentContainer;

        public StructureMapFilterProvider(Func<IContainer> container)
        {
            this.currentContainer = container;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            var container = this.currentContainer();

            foreach (var filter in filters)
            {
                container.BuildUp(filter.Instance);
                yield return filter;
            }
        }
    }
}