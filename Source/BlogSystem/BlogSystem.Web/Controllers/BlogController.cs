namespace BlogSystem.Web.Controllers
{
    using System.Web.Mvc;

    public class BlogController : Controller
    {
        public ActionResult Post(int id)
        {
            return this.Content(id.ToString());
        }
    }
}