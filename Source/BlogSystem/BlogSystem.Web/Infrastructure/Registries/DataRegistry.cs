namespace BlogSystem.Web.Infrastructure.Registries
{
    using BlogSystem.Data;
    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Repositories.Base;

    using StructureMap.Configuration.DSL;

    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<IApplicationDbContext>().Use<ApplicationDbContext>();
            AddType(typeof(IRepository<>), typeof(GenericRepository<>));
        }
    }
}
