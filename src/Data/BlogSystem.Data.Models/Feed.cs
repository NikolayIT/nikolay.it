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

        public string UrlRegexFilter { get; set; }

        public string NameRegexFilter { get; set; }

        public DateTime? LastUpdate { get; set; }

        [Display(Name = "Interval")]
        [Range(1, 7 * 24 * 60)]
        public int UpdateIntervalInMinutes { get; set; }

        [Display(Name = "Email")]
        public bool NotifyByEmail { get; set; }

        public ICollection<FeedItem> Items { get; set; }
    }
}
