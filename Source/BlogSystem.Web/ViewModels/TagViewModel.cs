namespace BlogSystem.Web.ViewModels
{
    using AutoMapper;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class TagViewModel : IMapFrom<Tag>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public int PostsCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Tag, TagViewModel>()
                .ForMember(m => m.PostsCount, opt => opt.MapFrom(u => u.BlogPosts.Count));
        }
    }
}