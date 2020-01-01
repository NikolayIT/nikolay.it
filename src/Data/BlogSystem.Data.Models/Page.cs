namespace BlogSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BlogSystem.Data.Common.Models;

    public class Page : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        [DataType(DataType.Html)]
        public string Content { get; set; }

        [DataType(DataType.MultilineText)]
        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        public string Permalink { get; set; }
    }
}
