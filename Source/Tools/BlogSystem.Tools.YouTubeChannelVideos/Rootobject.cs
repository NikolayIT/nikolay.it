namespace BlogSystem.Tools.YouTubeChannelVideos
{
    public class Rootobject
    {
        public string kind { get; set; }

        public string etag { get; set; }

        public string nextPageToken { get; set; }

        public Pageinfo pageInfo { get; set; }

        public Item[] items { get; set; }
    }
}