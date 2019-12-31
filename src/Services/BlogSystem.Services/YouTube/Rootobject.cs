namespace BlogSystem.Services.YouTube
{
    public class RootObject
    {
        public string Kind { get; set; }

        public string ETag { get; set; }

        public string NextPageToken { get; set; }

        public PageInfo PageInfo { get; set; }

        public Item[] Items { get; set; }
    }
}
