namespace BlogSystem.Data.Models
{
    using BlogSystem.Data.Common.Models;

    public class Page : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        public string Permalink { get; set; }
    }
}
