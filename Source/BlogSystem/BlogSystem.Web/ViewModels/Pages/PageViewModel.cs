namespace BlogSystem.Web.ViewModels.Pages
{
    using System;

    public class PageViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime LastModified { get; set; }
    }
}