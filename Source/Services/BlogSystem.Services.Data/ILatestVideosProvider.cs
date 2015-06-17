namespace BlogSystem.Services.Data
{
    using System.Linq;

    using BlogSystem.Data.Models;

    public interface ILatestVideosProvider
    {
        IQueryable<Video> GetLatestVideos(int count, string youtubeChannelId);
    }
}