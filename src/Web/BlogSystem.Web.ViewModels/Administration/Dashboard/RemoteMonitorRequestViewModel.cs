namespace BlogSystem.Web.ViewModels.Administration.Dashboard
{
    using System.Text.RegularExpressions;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class RemoteMonitorRequestViewModel : IMapFrom<RemoteMonitorRequest>
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string RegexPattern { get; set; }

        public string ActualValue { get; set; }

        public string ActualValueShort =>
            this.ActualValue.Length <= 100 ? this.ActualValue : this.ActualValue.Substring(0, 100) + "...";

        public bool Ok => Regex.Match(this.ActualValue, this.RegexPattern).Success;
    }
}
