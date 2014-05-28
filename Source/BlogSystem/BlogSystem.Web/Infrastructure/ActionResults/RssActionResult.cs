namespace BlogSystem.Web.Infrastructure.ActionResults
{
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;
    using System.Xml;

    public class RssActionResult : ActionResult
    {
        public RssActionResult(SyndicationFeed feed)
        {
            this.Feed = feed;
        }

        private SyndicationFeed Feed { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            var rssFormatter = new Rss20FeedFormatter(this.Feed);
            using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }
}