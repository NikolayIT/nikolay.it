namespace BlogSystem.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<RemoteMonitorRequestViewModel> RemoteMonitorRequests { get; set; }
    }
}
