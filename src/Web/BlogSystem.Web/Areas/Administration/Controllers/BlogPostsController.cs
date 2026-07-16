namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Administration;
    using BlogSystem.Web.ViewModels.Administration.BlogPosts;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class BlogPostsController : AdministrationController
    {
        private const int ItemsPerPage = 20;

        private readonly IDeletableEntityRepository<BlogPost> blogPosts;

        public BlogPostsController(IDeletableEntityRepository<BlogPost> blogPosts)
        {
            this.blogPosts = blogPosts;
        }

        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            var query = this.blogPosts.AllWithDeleted();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Title.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)totalCount / ItemsPerPage));
            page = Math.Clamp(page, 1, totalPages);

            var viewModel = new PaginatedListViewModel<BlogPostRowViewModel>
            {
                Items = await query
                    .OrderByDescending(x => x.CreatedOn)
                    .Skip((page - 1) * ItemsPerPage)
                    .Take(ItemsPerPage)
                    .To<BlogPostRowViewModel>()
                    .ToListAsync(),
                PageNumber = page,
                PageSize = ItemsPerPage,
                TotalCount = totalCount,
                SearchTerm = searchTerm,
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View(new BlogPostInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var blogPost = new BlogPost();
            CopyInputToEntity(input, blogPost);

            await this.blogPosts.AddAsync(blogPost);
            await this.blogPosts.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Blog post \"{blogPost.Title}\" was created.";
            return this.RedirectToAction(nameof(this.Edit), new { id = blogPost.Id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await this.blogPosts.AllWithDeleted()
                .Where(x => x.Id == id)
                .To<BlogPostEditViewModel>()
                .FirstOrDefaultAsync();
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPostEditViewModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            var blogPost = await this.blogPosts.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (blogPost == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                input.IsDeleted = blogPost.IsDeleted;
                input.CreatedOn = blogPost.CreatedOn;
                input.ModifiedOn = blogPost.ModifiedOn;
                return this.View(input);
            }

            CopyInputToEntity(input, blogPost);
            await this.blogPosts.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Blog post \"{blogPost.Title}\" was saved.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string searchTerm, int page = 1)
        {
            var blogPost = await this.blogPosts.All().FirstOrDefaultAsync(x => x.Id == id);
            if (blogPost == null)
            {
                return this.NotFound();
            }

            this.blogPosts.Delete(blogPost);
            await this.blogPosts.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Blog post \"{blogPost.Title}\" was deleted. You can restore it at any time.";
            return this.RedirectToAction(nameof(this.Index), new { searchTerm, page });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id, string searchTerm, int page = 1)
        {
            var blogPost = await this.blogPosts.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (blogPost == null)
            {
                return this.NotFound();
            }

            this.blogPosts.Undelete(blogPost);
            await this.blogPosts.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Blog post \"{blogPost.Title}\" was restored.";
            return this.RedirectToAction(nameof(this.Index), new { searchTerm, page });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int id, string searchTerm, int page = 1)
        {
            var blogPost = await this.blogPosts.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (blogPost == null)
            {
                return this.NotFound();
            }

            if (!blogPost.IsDeleted)
            {
                this.TempData["ErrorMessage"] = "Only deleted blog posts can be deleted permanently.";
                return this.RedirectToAction(nameof(this.Index), new { searchTerm, page });
            }

            this.blogPosts.HardDelete(blogPost);
            await this.blogPosts.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Blog post \"{blogPost.Title}\" was permanently deleted.";
            return this.RedirectToAction(nameof(this.Index), new { searchTerm, page });
        }

        private static void CopyInputToEntity(BlogPostInputModel input, BlogPost blogPost)
        {
            blogPost.Title = input.Title;
            blogPost.Type = input.Type;
            blogPost.ImageOrVideoUrl = input.ImageOrVideoUrl;
            blogPost.ShortContent = input.ShortContent;
            blogPost.Content = input.Content;
            blogPost.MetaDescription = input.MetaDescription;
            blogPost.MetaKeywords = input.MetaKeywords;
            blogPost.IsPinned = input.IsPinned;
        }
    }
}
