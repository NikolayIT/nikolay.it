namespace BlogSystem.Web.ViewModels.Videos
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class VideoListItemViewModel : IMapFrom<Video>
    {
        public int Id { get; set; }

        public string VideoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string VideoUrl => $"https://www.youtube.com/watch?v={this.VideoId}";

        public string ThumbnailUrl => $"https://i.ytimg.com/vi/{this.VideoId}/mqdefault.jpg";

        public string SmallThumbnailUrl => $"https://i.ytimg.com/vi/{this.VideoId}/default.jpg";
    }
}
