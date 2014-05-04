namespace BlogSystem.Data.Models
{
    public class BlogPost : ContentHolder
    {
        public int OldId { get; set; }

        public string ShortContent { get; set; }

        public string ImageOrVideoUrl { get; set; }
    }
}
