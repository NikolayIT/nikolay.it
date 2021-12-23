namespace BlogSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BlogSystem.Data.Common.Models;

    public class Feed : BaseModel<int>
    {
        public Feed()
        {
            this.Items = new HashSet<FeedItem>();
        }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        public FeedType Type { get; set; }

        public bool IsEnabled { get; set; }

        public string ItemsSelector { get; set; }

        public string Cookies { get; set; }

        public string PostData { get; set; }

        public string UrlRegexFilter { get; set; }

        public string NameRegexFilter { get; set; }

        public DateTime? LastUpdate { get; set; }

        [Range(1, 7 * 24 * 60)]
        public int UpdateIntervalInMinutes { get; set; }

        public bool NotifyByEmail { get; set; }

        public ICollection<FeedItem> Items { get; set; }
    }
}
