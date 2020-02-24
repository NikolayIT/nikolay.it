namespace BlogSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public IEnumerable<Video> GetLatestVideos(int count, string channelId)
        {
            this.FetchLatestVideosAsync(channelId).GetAwaiter().GetResult();
            return this.videosRepository.All().OrderByDescending(x => x.CreatedOn).Take(count).ToList();
        }

        public async Task FetchLatestVideosAsync(string youtubeChannelId)
        {
            var items = await this.videosFetcher.GetAllVideosFromChannel(
                            youtubeChannelId,
                            x => x.Snippet.Description.ToLower().Contains("николай")
                                 && x.Snippet.Description.ToLower().Contains("костов"));

            foreach (var item in items)
            {
                if (!this.videosRepository.All().Any(x => x.VideoId == item.Id.VideoId))
                {
                    var video = new Video
                    {
                        VideoId = item.Id.VideoId,
                        Title = item.Snippet.Title,
                        Description = item.Snippet.Description,
                        CreatedOn = item.Snippet.PublishedAt,
                    };

                    await this.videosRepository.AddAsync(video);
                }
            }

            await this.videosRepository.SaveChangesAsync();
        }
    }
}
