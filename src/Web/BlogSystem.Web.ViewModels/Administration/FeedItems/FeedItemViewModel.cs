namespace BlogSystem.Web.ViewModels.Administration.FeedItems
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class FeedItemViewModel : IMapFrom<FeedItem>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string FaviconUrl => !string.IsNullOrWhiteSpace(this.Url) && this.Url.StartsWith("http")
            ? "https://icons.feedercdn.com/" + new Uri(this.Url).Host
            : "/img/background.jpg";

        public bool IsRead { get; set; }

        public DateTime CreatedOn { get; set; }

        public int FeedId { get; set; }

        public string FeedName { get; set; }

        public string FeedUrl { get; set; }
    }
}
