namespace BlogSystem.Data.Models
{
    using System.Collections.Generic;

    public class Tag
    {
        private ICollection<BlogPost> blogPosts;
        private ICollection<Page> pages;

        public Tag()
        {
            this.blogPosts = new HashSet<BlogPost>();
            this.pages = new HashSet<Page>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        
        public virtual ICollection<BlogPost> BlogPosts
        {
            get { return this.blogPosts; }
            set { this.blogPosts = value; }
        }

        public virtual ICollection<Page> Pages
        {
            get { return this.pages; }
            set { this.pages = value; }
        }
    }
}
