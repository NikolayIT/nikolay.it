namespace BlogSystem.Web.ViewModels.Administration.Feeds
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class FeedViewModel : IMapFrom<Feed>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public FeedType Type { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? LastUpdate { get; set; }

        [Display(Name = "Interval")]
        public int UpdateIntervalInMinutes { get; set; }

        [Display(Name = "Email")]
        public bool NotifyByEmail { get; set; }

        [Display(Name = "Items")]
        public int ItemsCount { get; set; }
    }
}
