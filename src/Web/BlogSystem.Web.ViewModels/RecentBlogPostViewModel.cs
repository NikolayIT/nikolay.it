namespace BlogSystem.Web.ViewModels
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services;
    using BlogSystem.Services.Mapping;

    public class RecentBlogPostViewModel : IMapFrom<BlogPost>
    {
        private readonly IBlogUrlGenerator urlGenerator;

        public RecentBlogPostViewModel()
            : this(new BlogUrlGenerator())
        {
        }

        public RecentBlogPostViewModel(IBlogUrlGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public BlogPostType Type { get; set; }

        public DateTime CreatedOn { get; set; }

        public string IconClass
        {
            get
            {
                if (this.Type == BlogPostType.Video)
                {
                    return "fa-youtube-play";
                }
                else
                {
                    return "fa-file-text";
                }
            }
        }

        public string Url => this.urlGenerator.GenerateUrl(this.Id, this.Title, this.CreatedOn);
    }
}
