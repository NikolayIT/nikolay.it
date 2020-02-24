namespace BlogSystem.Services.YouTube
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class YouTubeChannelVideosFetcher : IYouTubeChannelVideosFetcher
    {
        private const string UrlFormat =
            "https://www.googleapis.com/youtube/v3/search?part=snippet&channelId={1}&key={0}&maxResults={2}&pageToken={3}";

        private readonly string apiKey;

        public YouTubeChannelVideosFetcher(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<IEnumerable<Item>> GetAllVideosFromChannel(string channelId, Func<Item, bool> predicate)
        {
            var items = new List<Item>();
            var nextPage = string.Empty;
            var client = new HttpClient();
            var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
            do
            {
                var url = string.Format(UrlFormat, this.apiKey, channelId, 50, nextPage);
                var response = await client.GetAsync(url);
                var jsonText = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<VideoResults>(jsonText, jsonSerializerOptions);
                items.AddRange(obj.Items.Where(x => x.Id.Kind == "youtube#video").Where(predicate));
                nextPage = obj.NextPageToken;
            }
            while (!string.IsNullOrWhiteSpace(nextPage));

            return items;
        }
    }
}
