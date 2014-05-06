namespace BlogSystem.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using StructureMap;

    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly Func<IContainer> factory;

        public StructureMapDependencyResolver(Func<IContainer> factory)
        {
            this.factory = factory;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }

            var container = this.factory();

            return serviceType.IsAbstract || serviceType.IsInterface
                ? container.TryGetInstance(serviceType)
                : container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.factory().GetAllInstances(serviceType).Cast<object>();
        }
    }
}