namespace BlogSystem.Web.ViewModels.Administration.Videos
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class VideoRowViewModel : IMapFrom<Video>
    {
        public int Id { get; set; }

        public string VideoId { get; set; }

        public string Title { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
