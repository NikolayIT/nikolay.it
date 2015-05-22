namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using BlogSystem.Common;
    using BlogSystem.Web.Areas.Administration.Helpers;
    using BlogSystem.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [ValidateInput(false)]
    public class AdminController : BaseController
    {
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return this.PartialView("_AdminMenu", AdminMenu.Items);
        }
    }
}
