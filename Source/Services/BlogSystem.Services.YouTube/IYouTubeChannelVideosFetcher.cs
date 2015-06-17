namespace BlogSystem.Services.YouTube
{
    using System;
    using System.Collections.Generic;

    using BlogSystem.Services.YouTube.Models;

    public interface IYouTubeChannelVideosFetcher
    {
        IEnumerable<Snippet> GetLatestVideosFromChannel(string channelPlaylistId, Func<Item, bool> predicate);

        IEnumerable<Snippet> GetAllVideosFromChannel(string channelPlaylistId, Func<Item, bool> predicate);
    }
}
