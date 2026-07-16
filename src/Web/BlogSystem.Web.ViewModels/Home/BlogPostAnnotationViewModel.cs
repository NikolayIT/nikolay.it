namespace BlogSystem.Web.ViewModels.Home
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services;
    using BlogSystem.Services.Mapping;

    using Mapster;

    public class BlogPostAnnotationViewModel : BlogSystem.Services.Mapping.IMapFrom<BlogPost>, IHaveCustomMappings
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

        public string Url => this.urlGenerator.GenerateUrl(this.Id, this.Title, this.CreatedOn);

        public void CreateMappings(TypeAdapterConfig configuration)
        {
            configuration.NewConfig<BlogPost, BlogPostAnnotationViewModel>()
                .Map(dest => dest.Content, src => src.ShortContent);
        }
    }
}
