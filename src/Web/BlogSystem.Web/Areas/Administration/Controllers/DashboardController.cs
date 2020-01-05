namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Data;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        private readonly IDeletableEntityRepository<RemoteMonitorRequest> remoteMonitorRequestsRepository;

        public DashboardController(
            ISettingsService settingsService,
            IDeletableEntityRepository<RemoteMonitorRequest> remoteMonitorRequestsRepository)
        {
            this.settingsService = settingsService;
            this.remoteMonitorRequestsRepository = remoteMonitorRequestsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var remoteRequests =
                this.remoteMonitorRequestsRepository.All().To<RemoteMonitorRequestViewModel>().ToList();

            var httpClient = new HttpClient();
            foreach (var remoteRequest in remoteRequests)
            {
                var httpRequest = await httpClient.GetAsync(remoteRequest.Url);
                remoteRequest.ActualValue = await httpRequest.Content.ReadAsStringAsync();
            }

            var viewModel = new IndexViewModel
                                {
                                    SettingsCount = this.settingsService.GetCount(),
                                    RemoteMonitorRequests = remoteRequests,
                                };
            return this.View(viewModel);
        }
    }
}
