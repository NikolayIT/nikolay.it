namespace BlogSystem.Web.ViewModels.Administration.Feeds
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class FeedViewModel : IMapFrom<Feed>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public FeedType Type { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? LastUpdate { get; set; }

        public DateTime? LastItemOn { get; set; }

        [Display(Name = "Interval")]
        public int UpdateIntervalInMinutes { get; set; }

        [Display(Name = "Email")]
        public bool NotifyByEmail { get; set; }

        [Display(Name = "Items")]
        public int ItemsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Feed, FeedViewModel>()
                .ForMember(x => x.LastItemOn, opt => opt.MapFrom(x => x.Items.Select(x => x.CreatedOn).OrderByDescending(x => x).FirstOrDefault()));
        }
    }
}
