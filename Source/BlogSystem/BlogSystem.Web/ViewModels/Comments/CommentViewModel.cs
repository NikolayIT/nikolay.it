namespace BlogSystem.Web.ViewModels.Comments
{
    using System;

    using AutoMapper;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure.Mapping;
    using System.ComponentModel.DataAnnotations;

    public class CommentViewModel : IMapFrom<PostComment>, IHaveCustomMappings
    {
        public CommentViewModel()
        {
        }

        public CommentViewModel(int blogPostId)
        {
            this.BlogPostId = blogPostId;
        }

        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int BlogPostId { get; set; }

        public string User { get; set; }

        public DateTime CommentedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PostComment, CommentViewModel>()
                .ForMember(m => m.CommentedOn, opt => opt.MapFrom(u => u.CreatedOn));
            configuration.CreateMap<PostComment, CommentViewModel>()
                .ForMember(m => m.User, opt => opt.MapFrom(u => u.User.UserName));
        }
    }
}