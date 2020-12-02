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

        public bool IsRead { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FeedName { get; set; }

        public string FeedUrl { get; set; }
    }
}
