namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Data.Models;

    public class BlogPostsController : AdminController
    {
        public BlogPostsController(ApplicationDbContext data)
            : base(data)
        {
        }


        // GET: Administration/BlogPosts
        public ActionResult Index()
        {
            return View(Data.BlogPosts.ToList());
        }

        // GET: Administration/BlogPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = Data.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: Administration/BlogPosts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OldId,ShortContent,ImageOrVideoUrl,Title,SubTitle,Content,MetaDescription,MetaKeywords,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                Data.BlogPosts.Add(blogPost);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: Administration/BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = Data.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: Administration/BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OldId,ShortContent,ImageOrVideoUrl,Title,SubTitle,Content,MetaDescription,MetaKeywords,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                Data.Entry(blogPost).State = EntityState.Modified;
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: Administration/BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = Data.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: Administration/BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = Data.BlogPosts.Find(id);
            Data.BlogPosts.Remove(blogPost);
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
