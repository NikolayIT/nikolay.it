namespace BlogSystem.Data.Models
{
    using BlogSystem.Data.Common.Models;

    public class Video : BaseDeletableModel<int>
    {
        public string VideoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
