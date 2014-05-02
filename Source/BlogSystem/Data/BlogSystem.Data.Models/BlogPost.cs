namespace BlogSystem.Data.Models
{
    public class BlogPost : ContentHolder
    {
        public string ShortContent { get; set; }

        public string ImageOrVideoUrl { get; set; }
    }
}
