namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;

    public class BlogPostsController : AdminController
    {
        private readonly IRepository<BlogPost> blogPosts;

        public BlogPostsController(IRepository<BlogPost> blogPosts)
        {
            this.blogPosts = blogPosts;
        }

        // GET: Administration/BlogPosts
        public ActionResult Index()
        {
            return this.View(this.blogPosts.All().OrderByDescending(x => x.CreatedOn).ToList());
        }

        // GET: Administration/BlogPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogPost blogPost = this.blogPosts.GetById(id.Value);
            if (blogPost == null)
            {
                return this.HttpNotFound();
            }

            return this.View(blogPost);
        }

        // GET: Administration/BlogPosts/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ShortContent,ImageOrVideoUrl,Type,Title,SubTitle,Content,MetaDescription,MetaKeywords,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                this.blogPosts.Add(blogPost);
                this.blogPosts.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(blogPost);
        }

        // GET: Administration/BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogPost blogPost = this.blogPosts.GetById(id.Value);
            if (blogPost == null)
            {
                return this.HttpNotFound();
            }

            return this.View(blogPost);
        }

        // POST: Administration/BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShortContent,ImageOrVideoUrl,Type,Title,SubTitle,Content,MetaDescription,MetaKeywords,IsDeleted,DeletedOn,CreatedOn")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                this.blogPosts.Update(blogPost);
                this.blogPosts.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(blogPost);
        }

        // GET: Administration/BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogPost blogPost = this.blogPosts.GetById(id.Value);
            if (blogPost == null)
            {
                return this.HttpNotFound();
            }

            return this.View(blogPost);
        }

        // POST: Administration/BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = this.blogPosts.GetById(id);
            this.blogPosts.Delete(blogPost);
            this.blogPosts.SaveChanges();
            return this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.blogPosts.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
