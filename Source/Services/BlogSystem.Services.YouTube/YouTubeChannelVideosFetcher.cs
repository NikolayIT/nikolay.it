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
            "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId={1}&key={0}&maxResults=50&pageToken={2}";

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
                var jsonText = client.DownloadString(string.Format(UrlFormat, this.apiKey, channelPlaylistId, nextPage));
                var obj = JsonConvert.DeserializeObject<RootObject>(jsonText);
                items.AddRange(
                    obj.Items.Where(
                        x =>
                        x.Snippet.Description.ToLower().Contains("костов")
                        || x.Snippet.Description.ToLower().Contains("kostov")
                        || x.Snippet.Title.ToLower().Contains("ники") || x.Snippet.Title.ToLower().Contains("niki"))
                        .Select(x => x.Snippet));

                nextPage = obj.NextPageToken;
            }
            while (!string.IsNullOrWhiteSpace(nextPage));

            return items;
        } 
    }
}
