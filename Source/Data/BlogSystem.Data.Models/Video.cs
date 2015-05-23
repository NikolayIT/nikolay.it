namespace BlogSystem.Data.Models
{
    using System;

    using BlogSystem.Data.Contracts;

    public class Video : DeletableEntity
    {
        public int Id { get; set; }

        public string VideoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public VideoProvider Provider { get; set; }
    }
}
