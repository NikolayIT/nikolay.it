namespace BlogSystem.Services.YouTube
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    using BlogSystem.Services.YouTube.Models;

    using Newtonsoft.Json;

    public class YouTubeChannelVideosFetcher : IYouTubeChannelVideosFetcher
    {
        private const string UrlFormat =
            "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId={1}&key={0}&maxResults={2}&pageToken={3}";

        private readonly string apiKey;

        public YouTubeChannelVideosFetcher(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public IEnumerable<Snippet> GetAllVideosFromChannel(string channelPlaylistId, Func<Item, bool> predicate)
        {
            var items = new List<Snippet>();
            var nextPage = string.Empty;
            var client = new WebClient { Encoding = Encoding.UTF8 };

            do
            {
                var jsonText = client.DownloadString(string.Format(UrlFormat, this.apiKey, channelPlaylistId, 50, nextPage));
                var obj = JsonConvert.DeserializeObject<RootObject>(jsonText);
                items.AddRange(obj.Items.Where(predicate).Select(x => x.Snippet));
                nextPage = obj.NextPageToken;
            }
            while (!string.IsNullOrWhiteSpace(nextPage));

            return items;
        }

        public IEnumerable<Snippet> GetLatestVideosFromChannel(string channelPlaylistId, int maxResults, Func<Item, bool> predicate)
        {
            var items = new List<Snippet>();
            var client = new WebClient { Encoding = Encoding.UTF8 };

            var jsonText = client.DownloadString(string.Format(UrlFormat, this.apiKey, channelPlaylistId, maxResults, string.Empty));
            var obj = JsonConvert.DeserializeObject<RootObject>(jsonText);
            items.AddRange(obj.Items.Where(predicate).Select(x => x.Snippet));

            return items;
        }
    }
}
