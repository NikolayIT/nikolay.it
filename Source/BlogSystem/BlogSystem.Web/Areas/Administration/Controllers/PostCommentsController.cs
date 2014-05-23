namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BlogSystem.Data.Models;
using BlogSystem.Data.Contracts;

    public class PostCommentsController : Controller
    {
        private readonly IRepository<PostComment> commentsData;
        private readonly IRepository<BlogPost> blogPostsData;

        public PostCommentsController(IRepository<PostComment> commentsRepository, IRepository<BlogPost> blogPostsRepository)
        {
            commentsData = commentsRepository;
            blogPostsData = blogPostsRepository;
        }

        // GET: /Administration/PostComments/
        public ActionResult Index()
        {
            var postcomments = commentsData.All().Include(p => p.BlogPost);
            return View(postcomments.ToList());
        }

        // GET: /Administration/PostComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostComment postcomment = commentsData.GetById(id.Value);
            if (postcomment == null)
            {
                return HttpNotFound();
            }
            return View(postcomment);
        }

        // GET: /Administration/PostComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostComment postcomment = commentsData.GetById(id.Value);
            if (postcomment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogPostId = new SelectList(blogPostsData.All(), "Id", "ShortContent", postcomment.BlogPostId);
            return View(postcomment);
        }

        // POST: /Administration/PostComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Content,BlogPostId,IsVisible,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")] PostComment postcomment)
        {
            if (ModelState.IsValid)
            {
                commentsData.Update(postcomment);
                commentsData.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogPostId = new SelectList(blogPostsData.All(), "Id", "ShortContent", postcomment.BlogPostId);
            return View(postcomment);
        }

        // GET: /Administration/PostComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostComment postcomment = commentsData.GetById(id.Value);
            if (postcomment == null)
            {
                return HttpNotFound();
            }
            return View(postcomment);
        }

        // POST: /Administration/PostComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostComment postcomment = commentsData.GetById(id);
            commentsData.Delete(postcomment);
            commentsData.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                commentsData.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
