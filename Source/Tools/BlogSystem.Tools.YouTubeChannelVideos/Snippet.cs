namespace BlogSystem.Tools.YouTubeChannelVideos
{
    using System;

    public class Snippet
    {
        public DateTime publishedAt { get; set; }

        public string channelId { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public Thumbnails thumbnails { get; set; }

        public string channelTitle { get; set; }

        public string playlistId { get; set; }

        public int position { get; set; }

        public Resourceid resourceId { get; set; }
    }
}