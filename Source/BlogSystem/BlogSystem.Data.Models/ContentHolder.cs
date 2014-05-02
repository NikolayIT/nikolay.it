namespace BlogSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BlogSystem.Data.Contracts;

    public class ContentHolder : DeletableEntity
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Content { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }
    }
}
