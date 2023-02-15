namespace BlogSystem.Services.Data.CronJobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;

    using AngleSharp.Html.Parser;
    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Messaging;
    using BlushingPenguin.JsonPath;
    using Hangfire;
    using Hangfire.Console;
    using Hangfire.Server;
    using Microsoft.EntityFrameworkCore;

    [AutomaticRetry(Attempts = 1)]
    public class CheckFeedsJob
    {
        private readonly IRepository<Feed> feedsRepository;

        private readonly IDeletableEntityRepository<FeedItem> feedItemsRepository;

        private readonly IEmailSender emailSender;

        private readonly HtmlParser parser;

        public CheckFeedsJob(
            IRepository<Feed> feedsRepository,
            IDeletableEntityRepository<FeedItem> feedItemsRepository,
            IEmailSender emailSender)
        {
            this.feedsRepository = feedsRepository;
            this.feedItemsRepository = feedItemsRepository;
            this.emailSender = emailSender;
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
                            items = await this.CheckRssAsync(feed);
                            break;
                        case FeedType.Html:
                            if (feed.Url.Contains("{page}"))
                            {
                                for (var page = 1; page <= 4; page++)
                                {
                                    items = items.Concat(
                                        await this.CheckHtmlAsync(feed.Url.Replace("{page}", page.ToString()), feed));
                                }
                            }
                            else
                            {
                                items = await this.CheckHtmlAsync(feed.Url, feed);
                            }

                            break;
                        case FeedType.Json:
                            items = await this.CheckJsonAsync(feed);
                            break;
                        case FeedType.OkStatusCode:
                            items = await this.CheckOkStatusCodeAsync(feed.Url);
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

        private async Task<IEnumerable<FeedItem>> CheckRssAsync(Feed feed)
        {
            var xml = await this.GetHttpContentAsync(feed.Url, feed);
            var remoteFeed = CodeHollow.FeedReader.FeedReader.ReadFromString(xml);
            var items = new List<FeedItem>();
            foreach (var item in remoteFeed.Items)
            {
                items.Add(new FeedItem { Title = item.Title, Url = item.Link, });
            }

            return items;
        }

        private async Task<IEnumerable<FeedItem>> CheckHtmlAsync(string feedUrl, Feed feed)
        {
            var html = await this.GetHttpContentAsync(feedUrl, feed);
            var document = await this.parser.ParseDocumentAsync(html);
            var elements = document.QuerySelectorAll(feed.ItemsSelector);
            var items = new List<FeedItem>();
            foreach (var element in elements)
            {
                var title = Regex.Replace(element.TextContent, @"\s+", " ").Trim();
                var url = element.Attributes["href"]?.Value ?? element.QuerySelector("a")?.Attributes["href"]?.Value
                          ?? $"{feedUrl}#{element.QuerySelector("a")?.Attributes["id"]?.Value}";

                if (!string.IsNullOrWhiteSpace(url))
                {
                    url = this.NormalizeUrl(url, feedUrl);
                }

                items.Add(new FeedItem { Title = title, Url = url, });
            }

            return items;
        }

        private async Task<IEnumerable<FeedItem>> CheckJsonAsync(Feed feed)
        {
            var json = await this.GetHttpContentAsync(feed.Url, feed);
            var document = JsonDocument.Parse(json);
            var elements = document.SelectTokens(feed.ItemsSelector);
            return elements.Select(element => new FeedItem { Title = element.GetString(), Url = feed.Url }).ToList();
        }

        private async Task<string> GetHttpContentAsync(string feedUrl, Feed feed)
        {
            var cookieContainer = new CookieContainer();
            if (!string.IsNullOrWhiteSpace(feed.Cookies))
            {
                var cookies = feed.Cookies.Split(';');
                foreach (var cookie in cookies)
                {
                    var cookieParts = cookie.Trim().Split(new char[] { '=' }, 2);
                    cookieContainer.Add(new Cookie(cookieParts[0], cookieParts[1], "/", new Uri(feedUrl).Host));
                }
            }

            var handler = new HttpClientHandler() { CookieContainer = cookieContainer, };
            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

            HttpResponseMessage response;
            if (!string.IsNullOrWhiteSpace(feed.PostData) && feed.PostData.StartsWith("{"))
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(feed.PostData, Encoding.UTF8, "application/json");
                response = await httpClient.PostAsync(feedUrl, content);
            }
            else if (!string.IsNullOrWhiteSpace(feed.PostData))
            {
                string postData = HttpUtility.UrlEncode(feed.PostData);
                byte[] data = Encoding.UTF8.GetBytes(postData);
                ByteArrayContent content = new ByteArrayContent(data);
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                response = await httpClient.PostAsync(feedUrl, content);
            }
            else
            {
                response = await httpClient.GetAsync(feedUrl);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Status code does not indicate success: {(int)response.StatusCode} {response.ReasonPhrase} \"{await response.Content.ReadAsStringAsync()}\"");
            }

            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        private async Task<IEnumerable<FeedItem>> CheckOkStatusCodeAsync(string feedUrl)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(feedUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<FeedItem>
                               {
                                   new() { Title = $"[{DateTime.UtcNow}] ({(int)response.StatusCode}) {response.StatusCode}", Url = feedUrl },
                               };
                }

                return new List<FeedItem>();
            }
            catch (Exception e)
            {
                return new List<FeedItem>
                           {
                               new() { Title = $"[{DateTime.UtcNow}] Exception: {e.Message}", Url = feedUrl },
                           };
            }
        }

        private string NormalizeUrl(string url, string baseUrl) =>
            string.IsNullOrWhiteSpace(url) ? null :
            Uri.TryCreate(new Uri(baseUrl), url, out var result) ? result.ToString() : url;
    }
}
