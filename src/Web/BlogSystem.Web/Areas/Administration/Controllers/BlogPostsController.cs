namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class BlogPostsController : AdministrationController
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPosts;

        public BlogPostsController(IDeletableEntityRepository<BlogPost> blogPosts)
        {
            this.blogPosts = blogPosts;
        }

        // GET: Administration/BlogPosts
        public async Task<IActionResult> Index()
        {
            return this.View(await this.blogPosts.All().OrderByDescending(x => x.CreatedOn).ToListAsync());
        }

        // GET: Administration/BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var blogPost = await this.blogPosts.All().FirstOrDefaultAsync(m => m.Id == id);
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
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(blogPost);
        }

        // GET: Administration/BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var blogPost = await this.blogPosts.All().FirstOrDefaultAsync(x => x.Id == id);
            if (blogPost == null)
            {
                return this.NotFound();
            }

            return this.View(blogPost);
        }

        // POST: Administration/BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.blogPosts.Update(blogPost);
                    await this.blogPosts.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.BlogPostExists(blogPost.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(blogPost);
        }

        // GET: Administration/BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var blogPost = await this.blogPosts.All().FirstOrDefaultAsync(m => m.Id == id);
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
            var blogPost = await this.blogPosts.All().FirstOrDefaultAsync(x => x.Id == id);
            this.blogPosts.Delete(blogPost);
            await this.blogPosts.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool BlogPostExists(int id)
        {
            return this.blogPosts.All().Any(e => e.Id == id);
        }
    }
}
