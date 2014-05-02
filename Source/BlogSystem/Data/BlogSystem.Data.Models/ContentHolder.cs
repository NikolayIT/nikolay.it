namespace BlogSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BlogSystem.Data.Contracts;

    public class ContentHolder : DeletableEntity
    {
        private ICollection<Tag> tags;

        public ContentHolder()
        {
            this.tags = new HashSet<Tag>();
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Content { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        public virtual ICollection<Tag> SubmissionTypes
        {
            get { return this.tags; }
            set { this.tags = value; }
        }
    }
}
