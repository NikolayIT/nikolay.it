namespace BlogSystem.Services
{
    using System;
    using System.Text.RegularExpressions;

    public class YouTubeUrlParser : IYouTubeUrlParser
    {
        private static readonly Regex VideoIdRegex = new Regex(
            @"(?:youtube\.com/(?:embed/|v/|watch\?v=|watch\?.*?&v=)|youtu\.be/)([A-Za-z0-9_-]{11})",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public string GetVideoId(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            var match = VideoIdRegex.Match(url);
            return match.Success ? match.Groups[1].Value : null;
        }

        public string GetThumbnailUrl(string url)
        {
            var videoId = this.GetVideoId(url);
            return videoId == null ? null : $"https://img.youtube.com/vi/{videoId}/maxresdefault.jpg";
        }

        public string ToAbsoluteImageUrl(string imageOrVideoUrl, string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(imageOrVideoUrl))
            {
                return null;
            }

            var thumbnail = this.GetThumbnailUrl(imageOrVideoUrl);
            if (thumbnail != null)
            {
                return thumbnail;
            }

            // Uri.TryCreate(.., UriKind.Absolute) is not usable here: on Linux it parses "/img/x.png"
            // as an absolute file: URI, so the path would be returned unchanged on the server.
            if (imageOrVideoUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || imageOrVideoUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return imageOrVideoUrl;
            }

            return baseUrl?.TrimEnd('/') + "/" + imageOrVideoUrl.TrimStart('/');
        }
    }
}
