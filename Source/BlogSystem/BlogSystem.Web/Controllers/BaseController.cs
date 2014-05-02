namespace BlogSystem.Web.Controllers
{
    using System.Web.Mvc;

    using BlogSystem.Data;

    public class BaseController : Controller
    {
        public BaseController(ApplicationDbContext data)
        {
            this.Data = data;
        }

        public ApplicationDbContext Data { get; set; }
    }
}