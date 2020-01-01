namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;

    using Microsoft.AspNetCore.Mvc;

    public class BlogPostsController : AdministrationController
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPosts;

        public BlogPostsController(IDeletableEntityRepository<BlogPost> blogPosts)
        {
            this.blogPosts = blogPosts;
        }

        // GET: Administration/BlogPosts
        public IActionResult Index()
        {
            return this.View(this.blogPosts.All().OrderByDescending(x => x.CreatedOn).ToList());
        }

        // GET: Administration/BlogPosts/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var blogPost = this.blogPosts.All().FirstOrDefault(x => x.Id == id);
            if (blogPost == null)
            {
                return this.NotFound();
            }

            return this.View(blogPost);
        }

        // GET: Administration/BlogPosts/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPost blogPost)
        {
            if (this.ModelState.IsValid)
            {
                await this.blogPosts.AddAsync(blogPost);
                await this.blogPosts.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(blogPost);
        }

        // GET: Administration/BlogPosts/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var blogPost = this.blogPosts.All().FirstOrDefault(x => x.Id == id);
            if (blogPost == null)
            {
                return this.NotFound();
            }

            return this.View(blogPost);
        }

        // POST: Administration/BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogPost blogPost)
        {
            if (this.ModelState.IsValid)
            {
                this.blogPosts.Update(blogPost);
                await this.blogPosts.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(blogPost);
        }

        // GET: Administration/BlogPosts/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var blogPost = this.blogPosts.All().FirstOrDefault(x => x.Id == id);
            if (blogPost == null)
            {
                return this.NotFound();
            }

            return this.View(blogPost);
        }

        // POST: Administration/BlogPosts/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = this.blogPosts.All().FirstOrDefault(x => x.Id == id);
            this.blogPosts.Delete(blogPost);
            await this.blogPosts.SaveChangesAsync();
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
