namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using BlogSystem.Common;
    using BlogSystem.Data;
    using BlogSystem.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [ValidateInput(false)]
    public abstract class AdminController : BaseController
    {
        protected AdminController(ApplicationDbContext data)
            : base(data)
        {
        }
    }
}
