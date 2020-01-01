namespace BlogSystem.Services.Data
{
    using System.Collections.Generic;

    using BlogSystem.Data.Models;

    public interface ILatestVideosProvider
    {
        IEnumerable<Video> GetLatestVideos(int count, string youtubeChannelId);
    }
}
