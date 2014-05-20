namespace BlogSystem.Web.ViewModels.Home
{
    using System;

    using AutoMapper;

    using BlogSystem.Data.Models;
    using BlogSystem.Web.Helpers;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class BlogPostAnnotationViewModel : IMapFrom<BlogPost>, IHaveCustomMappings
    {
        private readonly IBlogUrlGenerator urlGenerator;

        public BlogPostAnnotationViewModel()
            : this(new BlogUrlGenerator())
        {
        }

        public BlogPostAnnotationViewModel(IBlogUrlGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageOrVideoUrl { get; set; }

        public string Url
        {
            get
            {
                return this.urlGenerator.GenerateUrl(this.Id, this.Title, this.CreatedOn);
            }
        }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<BlogPost, BlogPostAnnotationViewModel>()
                .ForMember(m => m.Content, opt => opt.MapFrom(u => u.ShortContent));
        }
    }
}