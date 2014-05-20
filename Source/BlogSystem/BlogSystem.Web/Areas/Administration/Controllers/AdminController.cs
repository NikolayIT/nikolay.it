namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using BlogSystem.Common;
    using BlogSystem.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [ValidateInput(false)]
    public abstract class AdminController : BaseController
    {
    }
}
