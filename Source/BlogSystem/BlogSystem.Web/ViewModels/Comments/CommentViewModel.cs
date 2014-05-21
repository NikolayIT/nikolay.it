namespace BlogSystem.Web.ViewModels.Comments
{
    using System;

    using AutoMapper;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class CommentViewModel : IMapFrom<PostComment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int BlogPostId { get; set; }

        public DateTime CommentedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PostComment, CommentViewModel>()
                .ForMember(m => m.CommentedOn, opt => opt.MapFrom(u => u.CreatedOn));
        }
    }
}