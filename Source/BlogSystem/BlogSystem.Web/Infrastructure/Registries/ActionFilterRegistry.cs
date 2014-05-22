namespace BlogSystem.Web.Infrastructure.Registries
{
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using System;
    using System.Web.Mvc;

    public class ActionFilterRegistry : Registry
    {
        public ActionFilterRegistry(Func<IContainer> containerFactory)
        {
            For<IFilterProvider>().Use(
                new StructureMapFilterProvider(containerFactory));

            Policies.SetAllProperties(
                x => x.Matching(p => 
                    p.DeclaringType.IsAssignableFrom(typeof(ActionFilterAttribute)) &&
                    p.DeclaringType.Namespace.StartsWith("BlogSystem") &&
                    !p.PropertyType.IsPrimitive &&
                    p.PropertyType != typeof(string)));
        }
    }
}