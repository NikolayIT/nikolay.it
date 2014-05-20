namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;

    public class PagesController : AdminController
    {
        private readonly IRepository<Page> pages; 

        public PagesController(IRepository<Page> pages)
        {
            this.pages = pages;
        }

        public ActionResult Index()
        {
            return this.View(this.pages.All().ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Page page = this.pages.GetById(id.Value);
            if (page == null)
            {
                return this.HttpNotFound();
            }

            return this.View(page);
        }

        public ActionResult Create()
        {
            return this.View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Permalink,Title,SubTitle,Content,MetaDescription,MetaKeywords,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")] Page page)
        {
            if (ModelState.IsValid)
            {
                this.pages.Add(page);
                this.pages.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(page);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Page page = this.pages.GetById(id.Value);
            if (page == null)
            {
                return this.HttpNotFound();
            }

            return this.View(page);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Permalink,Title,SubTitle,Content,MetaDescription,MetaKeywords,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")] Page page)
        {
            if (ModelState.IsValid)
            {
                this.pages.Update(page);
                this.pages.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(page);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Page page = this.pages.GetById(id.Value);
            if (page == null)
            {
                return this.HttpNotFound();
            }

            return this.View(page);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Page page = this.pages.GetById(id);
            this.pages.Delete(page);
            this.pages.SaveChanges();
            return this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.pages.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
