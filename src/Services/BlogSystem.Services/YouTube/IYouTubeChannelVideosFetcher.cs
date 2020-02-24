namespace BlogSystem.Services.YouTube
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IYouTubeChannelVideosFetcher
    {
        Task<IEnumerable<Item>> GetAllVideosFromChannel(string channelId, Func<Item, bool> predicate);
    }
}
