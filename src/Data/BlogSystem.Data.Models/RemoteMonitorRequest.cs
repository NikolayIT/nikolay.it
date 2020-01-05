namespace BlogSystem.Data.Models
{
    using BlogSystem.Data.Common.Models;

    public class RemoteMonitorRequest : BaseDeletableModel<int>
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public string RegexPattern { get; set; }
    }
}
