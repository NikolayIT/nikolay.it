namespace BlogSystem.Tools.YouTubeChannelVideos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    using BlogSystem.Data;
    using BlogSystem.Data.Models;

    using Newtonsoft.Json;

    // TODO: Improve code quality
    internal static class Program
    {
        private const string ApiKey = "{YOUR_API_KEY}";

        private const string ChannelPlaylistId = "UULC-vbm7OWvpbqzXaoAMGGw";

        private const string UrlFormat =
            "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId={1}&key={0}&maxResults=50&pageToken={2}";

        private static void Main()
        {
            var total = 0;
            var items = new List<Snippet>();
            var nextPage = string.Empty;
            var client = new WebClient { Encoding = Encoding.UTF8 };

            do
            {
                var jsonText = client.DownloadString(string.Format(UrlFormat, ApiKey, ChannelPlaylistId, nextPage));
                var obj = JsonConvert.DeserializeObject<RootObject>(jsonText);
                items.AddRange(
                    obj.Items.Where(
                        x =>
                        x.Snippet.Description.ToLower().Contains("костов")
                        || x.Snippet.Description.ToLower().Contains("kostov")
                        || x.Snippet.Title.ToLower().Contains("ники") || x.Snippet.Title.ToLower().Contains("niki"))
                        .Select(x => x.Snippet));
                total += obj.Items.Count();
                Console.WriteLine("{0} / {1}", items.Count, total);
                nextPage = obj.NextPageToken;
            }
            while (!string.IsNullOrWhiteSpace(nextPage));

            var db = new ApplicationDbContext();
            foreach (var item in items)
            {
                var videoId = item.ResourceId.VideoId;
                if (!db.Videos.Any(x => x.VideoId == videoId))
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

                    db.Videos.Add(video);
                }
            }

            db.SaveChanges();
        }
    }
}
