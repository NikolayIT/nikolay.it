namespace BlogSystem.Web.Infrastructure.Registries
{
    using BlogSystem.Web.Infrastructure.Conventions;

    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class ControllerRegistry : Registry
    {
        public ControllerRegistry()
        {
            this.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.With(new ControllerConvention());
            });
        }
    }
}
