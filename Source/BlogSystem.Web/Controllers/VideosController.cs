namespace BlogSystem.Web.Controllers
{
    using System.Web.Mvc;

    public class VideosController : Controller
    {
        // GET: Videos
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
