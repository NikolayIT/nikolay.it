namespace BlogSystem.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int SettingsCount { get; set; }

        public IEnumerable<RemoteMonitorRequestViewModel> RemoteMonitorRequests { get; set; }
    }
}
