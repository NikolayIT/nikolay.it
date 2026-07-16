namespace BlogSystem.Web.ViewModels.Administration
{
    using System;

    public class PaginationViewModel
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public string SearchTerm { get; set; }

        public int TotalPages => this.PageSize == 0 ? 0 : (int)Math.Ceiling((double)this.TotalCount / this.PageSize);

        public bool HasPreviousPage => this.PageNumber > 1;

        public bool HasNextPage => this.PageNumber < this.TotalPages;
    }
}
