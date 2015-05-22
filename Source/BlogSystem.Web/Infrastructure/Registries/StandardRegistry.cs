namespace BlogSystem.Web.Infrastructure.Registries
{
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class StandardRegistry : Registry
    {
        public StandardRegistry()
        {
            this.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
