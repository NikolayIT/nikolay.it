namespace BlogSystem.Web.Infrastructure.Registries
{
    using BlogSystem.Data;
    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;
    using BlogSystem.Data.Repositories.Base;

    using StructureMap.Configuration.DSL;

    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            // TODO: Register IRepository<T> to GenericRepository<T>
            For<IApplicationDbContext>().Use<ApplicationDbContext>();
            For<IRepository<BlogPost>>().Use<GenericRepository<BlogPost>>();
            For<IRepository<Page>>().Use<GenericRepository<Page>>();
            For<IRepository<Tag>>().Use<GenericRepository<Tag>>();
            For<IRepository<Setting>>().Use<GenericRepository<Setting>>();
        }
    }
}
