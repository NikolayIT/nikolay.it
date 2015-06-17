namespace BlogSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using BlogSystem.Data;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.YouTube;

    public class LatestVideosProvider : ILatestVideosProvider
    {
        private readonly IApplicationDbContext db;

        private readonly IYouTubeChannelVideosFetcher videosFetcher;

        public LatestVideosProvider(string youtubeApiKey)
            : this(new ApplicationDbContext(), new YouTubeChannelVideosFetcher(youtubeApiKey))
        {
        }

        public LatestVideosProvider(IApplicationDbContext db, IYouTubeChannelVideosFetcher videosFetcher)
        {
            this.db = db;
            this.videosFetcher = videosFetcher;
        }

        public IQueryable<Video> GetLatestVideos(int count, string youtubeChannelId)
        {
            this.FetchLatestVideo(youtubeChannelId);
            return this.GetLatestVideosFromDatabase(count);
        }

        private IQueryable<Video> GetLatestVideosFromDatabase(int count)
        {
            return this.db.Videos.OrderByDescending(x => x.CreatedOn).Take(count);
        }

        private void FetchLatestVideo(string youtubeChannelId)
        {
            var items = this.videosFetcher.GetAllVideosFromChannel(
                youtubeChannelId,
                x =>
                x.Snippet.Description.ToLower().Contains("костов") || x.Snippet.Description.ToLower().Contains("kostov")
                || x.Snippet.Title.ToLower().Contains("ники") || x.Snippet.Title.ToLower().Contains("niki"));

            foreach (var item in items)
            {
                var videoId = item.ResourceId.VideoId;
                if (!this.db.Videos.Any(x => x.VideoId == videoId))
                {
                    var video = new Video
                    {
                        VideoId = videoId,
                        Title = item.Title,
                        Description = item.Description,
                        PreserveCreatedOn = true,
                        CreatedOn = item.PublishedAt,
                        Provider = VideoProvider.YouTube
                    };

                    this.db.Videos.Add(video);
                }
            }

            this.db.SaveChanges();
        }
    }
}
