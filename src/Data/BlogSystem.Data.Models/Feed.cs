using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BlogSystem.Data.Common.Models;

    // TODO: Add validation attributes
    public class Feed : BaseModel<int>
    {
        public Feed()
        {
            this.Items = new HashSet<FeedItem>();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public FeedType Type { get; set; }

        public bool IsEnabled { get; set; }

        public string ItemsSelector { get; set; }

        public string UrlRegexFilter { get; set; }

        public string NameRegexFilter { get; set; }

        public DateTime? LastUpdate { get; set; }

        [Display(Name = "Interval")]
        public int UpdateIntervalInMinutes { get; set; }

        [Display(Name = "Email")]
        public bool NotifyByEmail { get; set; }

        public ICollection<FeedItem> Items { get; set; }
    }
}
