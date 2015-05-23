namespace BlogSystem.Web.ViewModels.Videos
{
    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class VideoListItemViewModel : IMapFrom<Video>
    {
        public int Id { get; set; }

        public string VideoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public VideoProvider Provider { get; set; }

        public string VideoUrl
        {
            get
            {
                if (this.Provider == VideoProvider.YouTube)
                {
                    return string.Format("https://www.youtube.com/watch?v={0}", this.VideoId);
                }
                else
                {
                    return this.VideoId; // TODO: Or implement video URL in Video model when provider is not YouTube
                }
            }
        }

        public string ThumbnailUrl
        {
            get
            {
                if (this.Provider == VideoProvider.YouTube)
                {
                    return string.Format("https://i.ytimg.com/vi/{0}/mqdefault.jpg", this.VideoId);
                }
                else
                {
                    return this.VideoId; // TODO: Implement thumbnail URL in Video model when provider is not YouTube
                }
            }
        }
    }
}