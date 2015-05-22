namespace BlogSystem.Web.Infrastructure
{
    using System;
    using System.Threading;

    using BlogSystem.Web.Infrastructure.Registries;

    using StructureMap;

    public static class ObjectFactory
    {
        private static readonly Lazy<Container> ContainerBuilder = new Lazy<Container>(
            DefaultContainer,
            LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return ContainerBuilder.Value; }
        }

        private static Container DefaultContainer()
        {
            return new Container(cfg =>
                {
                    cfg.AddRegistry(new StandardRegistry());
                    cfg.AddRegistry(new ControllerRegistry());
                    cfg.AddRegistry(new ActionFilterRegistry(() => Container ?? Container));
                    cfg.AddRegistry(new MvcRegistry());

                    //// TODO: cfg.AddRegistry(new TaskRegistry());
                    cfg.AddRegistry(new DataRegistry());
                });
        }
    }
}
