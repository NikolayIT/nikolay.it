namespace BlogSystem.Services.Data.CronJobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using AngleSharp.Html.Parser;
    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Messaging;
    using BlushingPenguin.JsonPath;
    using Hangfire;
    using Hangfire.Console;
    using Hangfire.Server;
    using Microsoft.EntityFrameworkCore;

    public class CheckFeedsJob
    {
        private readonly IRepository<Feed> feedsRepository;

        private readonly IDeletableEntityRepository<FeedItem> feedItemsRepository;

        private readonly IEmailSender emailSender;

        private readonly HttpClient httpClient;

        private readonly HtmlParser parser;

        public CheckFeedsJob(
            IRepository<Feed> feedsRepository,
            IDeletableEntityRepository<FeedItem> feedItemsRepository,
            IEmailSender emailSender)
        {
            this.feedsRepository = feedsRepository;
            this.feedItemsRepository = feedItemsRepository;
            this.emailSender = emailSender;
            this.httpClient = new HttpClient();
            this.parser = new HtmlParser();
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task Work(PerformContext context)
        {
            var feeds = await this.feedsRepository.All()
                .Where(x => x.IsEnabled && (x.LastUpdate == null ||
                                            x.LastUpdate.Value.AddMinutes(x.UpdateIntervalInMinutes) < DateTime.UtcNow))
                .ToListAsync();
            foreach (var feed in feeds)
            {
                try
                {
                    IEnumerable<FeedItem> items = new List<FeedItem>();
                    switch (feed.Type)
                    {
                        case FeedType.Rss:
                            items = await this.CheckRssAsync(feed.Url);
                            break;
                        case FeedType.Html:
                            items = await this.CheckHtmlAsync(feed.Url, feed.ItemsSelector);
                            break;
                        case FeedType.Json:
                            items = await this.CheckJsonAsync(feed.Url, feed.ItemsSelector);
                            break;
                    }

                    foreach (var feedItem in items)
                    {
                        if (string.IsNullOrWhiteSpace(feedItem.Title))
                        {
                            continue;
                        }

                        if (this.feedItemsRepository.AllWithDeleted().Any(x =>
                            x.FeedId == feed.Id && x.Url == feedItem.Url && x.Title == feedItem.Title))
                        {
                            // This item exists
                            continue;
                        }

                        if (!string.IsNullOrWhiteSpace(feed.NameRegexFilter) &&
                            !Regex.IsMatch(feedItem.Title, feed.NameRegexFilter, RegexOptions.IgnoreCase))
                        {
                            // Filtered by name
                            continue;
                        }

                        if (!string.IsNullOrWhiteSpace(feed.UrlRegexFilter) && !string.IsNullOrWhiteSpace(feedItem.Url) &&
                            !Regex.IsMatch(feedItem.Url, feed.UrlRegexFilter, RegexOptions.IgnoreCase))
                        {
                            // Filtered by URL
                            continue;
                        }

                        feedItem.IsRead = false;
                        feedItem.FeedId = feed.Id;
                        await this.feedItemsRepository.AddAsync(feedItem);
                        await this.feedItemsRepository.SaveChangesAsync();

                        if (feed.NotifyByEmail && feed.LastUpdate.HasValue)
                        {
                            await this.emailSender.SendEmailAsync(
                                "blog@nikolay.it",
                                "Nikolay.IT Blog",
                                "feed@nikolay.it",
                                $"New item in {feed.Name}",
                                $"{feedItem.Title}<br />{feedItem.Url}");
                        }
                    }

                    feed.LastUpdate = DateTime.UtcNow;
                    await this.feedsRepository.SaveChangesAsync();

                    context.WriteLine($"Checked: {feed.Name}");
                }
                catch (Exception e)
                {
                    context.WriteLine($"Error: {feed.Name}: {e}");
                }
            }
        }

        private async Task<IEnumerable<FeedItem>> CheckRssAsync(string feedUrl)
        {
            var remoteFeed = await CodeHollow.FeedReader.FeedReader.ReadAsync(feedUrl);
            var items = new List<FeedItem>();
            foreach (var item in remoteFeed.Items)
            {
                items.Add(new FeedItem { Title = item.Title, Url = item.Link, });
            }

            return items;
        }

        private async Task<IEnumerable<FeedItem>> CheckHtmlAsync(string feedUrl, string itemsSelector)
        {
            var response = await this.httpClient.GetAsync(feedUrl);
            var html = await response.Content.ReadAsStringAsync();
            var document = await this.parser.ParseDocumentAsync(html);
            var elements = document.QuerySelectorAll(itemsSelector);
            var items = new List<FeedItem>();
            foreach (var element in elements)
            {
                var title = Regex.Replace(element.TextContent, @"\s+", " ").Trim();
                var url = element.QuerySelector("a")?.Attributes["href"]?.Value ??
                          $"{feedUrl}#{element.QuerySelector("a")?.Attributes["id"]?.Value}";

                if (!string.IsNullOrWhiteSpace(url))
                {
                    url = this.NormalizeUrl(url, feedUrl);
                }

                items.Add(new FeedItem { Title = title, Url = url, });
            }

            return items;
        }

        private async Task<IEnumerable<FeedItem>> CheckJsonAsync(string feedUrl, string itemsSelector)
        {
            var response = await this.httpClient.GetAsync(feedUrl);
            var json = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(json);
            var elements = document.SelectTokens(itemsSelector);
            return elements.Select(element => new FeedItem { Title = element.GetString(), Url = feedUrl }).ToList();
        }

        private string NormalizeUrl(string url, string baseUrl) =>
            string.IsNullOrWhiteSpace(url) ? null :
            Uri.TryCreate(new Uri(baseUrl), url, out var result) ? result.ToString() : url;
    }
}
