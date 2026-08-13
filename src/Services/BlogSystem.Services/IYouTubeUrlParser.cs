namespace BlogSystem.Services
{
    public interface IYouTubeUrlParser
    {
        string GetVideoId(string url);

        string GetThumbnailUrl(string url);

        string ToAbsoluteImageUrl(string imageOrVideoUrl, string baseUrl);
    }
}
