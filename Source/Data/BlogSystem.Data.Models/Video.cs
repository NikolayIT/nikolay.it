namespace BlogSystem.Data.Models
{
    public class Video
    {
        public int Id { get; set; }

        public string VideoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public VideoProvider Provider { get; set; }
    }
}
