namespace BlogSystem.Services.YouTube.Models
{
    using System;

    public class Snippet
    {
        public DateTime PublishedAt { get; set; }

        public string ChannelId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Thumbnails Thumbnails { get; set; }

        public string ChannelTitle { get; set; }

        public string PlaylistId { get; set; }

        public int Position { get; set; }

        public Resourceid ResourceId { get; set; }
    }
}