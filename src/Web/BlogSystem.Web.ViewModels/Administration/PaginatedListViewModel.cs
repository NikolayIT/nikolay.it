namespace BlogSystem.Web.ViewModels.Administration
{
    using System.Collections.Generic;

    public class PaginatedListViewModel<T> : PaginationViewModel
    {
        public IReadOnlyCollection<T> Items { get; set; }
    }
}
