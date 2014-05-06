namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Data.Models;

    public class PagesController : AdminController
    {
        public PagesController(ApplicationDbContext data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View(Data.Pages.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = Data.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        public ActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Permalink,Title,SubTitle,Content,MetaDescription,MetaKeywords,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")] Page page)
        {
            if (ModelState.IsValid)
            {
                Data.Pages.Add(page);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(page);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = Data.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Permalink,Title,SubTitle,Content,MetaDescription,MetaKeywords,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")] Page page)
        {
            if (ModelState.IsValid)
            {
                Data.Entry(page).State = EntityState.Modified;
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(page);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = Data.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Page page = Data.Pages.Find(id);
            Data.Pages.Remove(page);
            Data.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Data.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
