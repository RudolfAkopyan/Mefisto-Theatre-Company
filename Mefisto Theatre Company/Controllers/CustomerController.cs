using Mefisto_Theatre_Company.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Controllers
{
    public class CustomerController : Controller
    {
        private MefistoDBContext db = new MefistoDBContext();
        // GET: Member Comments
        public ActionResult Index()
        {
            // Retrieve comments for the currently logged-in user
            var comments = db.Comments.Include(c => c.Post).Include(c => c.User);
            var userId = User.Identity.GetUserId();
            comments = comments.Where(c => c.UserId == userId);
            return View(comments.ToList());
        }

        // GET: Comment/Details
        public ActionResult Details(int? id)
        {
            if (id == null)     
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);      // Return a bad request status if the ID is null
            }

            Comment comment = db.Comments
            .Include(c => c.Post)
            .Include(c => c.User)
            .SingleOrDefault(c => c.CommentId == id);

            if (comment == null)            // Return a not found status if the comment is not found
            {
                return HttpNotFound();

            }
            return View(comment);
        }


        // GET: Comment/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // Return a bad request status if the ID is null
            }

            Comment comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();      // Return a not found status if the comment is not found

            }
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Title", comment.PostId);
            return View(comment);
        }

        // POST: Member/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId, Description, PostId")] Comment comment)         // Handle the editing of a comment
        {
            if (ModelState.IsValid)
            {
                // Update the comment details and save changes
                comment.DatePosted = DateTime.Now;
                comment.UserId = User.Identity.GetUserId();
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Title", comment.PostId);
            return View(comment);
        }

        // GET: Delete a comment
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);      // Return a bad request status if the ID is null
            }
            // Retrieve the comment for deletion with associated post details
            Comment comment = db.Comments.Find(id);
            var post = db.Posts.Find(comment.PostId);
            comment.Post = post;

            if (comment == null)
            {
                return HttpNotFound();      // Return a not found status if the comment is not found

            }
            return View(comment);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET Comment/Create
        public ActionResult AddComment(int? id)
        {
            // Retrieve the post for which a comment is being added
            Post post = db.Posts.Find(id);
            Comment comment = new Comment();
            comment.Post = post;
            comment.PostId = post.PostId;
            return View(comment);

        }

        //POST Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handle the creation of a new comment
        public ActionResult AddComment(Comment comment)
        {
            // Set the date posted and user ID, add the comment to the database, and save changes
            if (ModelState.IsValid)
            {
                comment.DatePosted = DateTime.Now;
                comment.UserId = User.Identity.GetUserId();
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("AllPosts", "Home");

            }

            return View(comment);

        }
    }
}

