namespace BlogSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data;
    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.YouTube;

    public class LatestVideosProvider : ILatestVideosProvider
    {
        private readonly IRepository<Video> videosRepository;

        private readonly IYouTubeChannelVideosFetcher videosFetcher;

        public LatestVideosProvider(IRepository<Video> videosRepository, IYouTubeChannelVideosFetcher videosFetcher)
        {
            this.videosRepository = videosRepository;
            this.videosFetcher = videosFetcher;
        }

        public IEnumerable<Video> GetLatestVideos(int count, string youtubeChannelId)
        {
            this.FetchLatestVideosAsync(youtubeChannelId).GetAwaiter().GetResult();
            return this.GetLatestVideosFromDatabase(count);
        }

        private IQueryable<Video> GetLatestVideosFromDatabase(int count)
        {
            return this.videosRepository.All().OrderByDescending(x => x.CreatedOn).Take(count);
        }

        private async Task FetchLatestVideosAsync(string youtubeChannelId)
        {
            var items = this.videosFetcher.GetAllVideosFromChannel(
                youtubeChannelId,
                x => x.Snippet.Description.ToLower().Contains("костов")
                     || x.Snippet.Description.ToLower().Contains("kostov")
                     || x.Snippet.Title.ToLower().Contains("ники")
                     || x.Snippet.Title.ToLower().Contains("niki"));

            foreach (var item in items)
            {
                var videoId = item.ResourceId.VideoId;
                if (!this.videosRepository.All().Any(x => x.VideoId == videoId))
                {
                    var video = new Video
                    {
                        VideoId = videoId,
                        Title = item.Title,
                        Description = item.Description,
                        CreatedOn = item.PublishedAt,
                    };

                    await this.videosRepository.AddAsync(video);
                }
            }

            await this.videosRepository.SaveChangesAsync();
        }
    }
}
