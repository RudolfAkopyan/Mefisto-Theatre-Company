using Mefisto_Theatre_Company.Models;
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
    public class CommentsController : Controller
    {
        private MefistoDBContext db = new MefistoDBContext();

        // GET: Comments
        public ActionResult Index()
        {
            //var comments = db.Comments.Include(c => c.Post).Include(c => c.User).Where(c=>c.IsApproved==false);
            var comments = db.Comments.Include(c => c.Post).Include(c => c.User);
            return View(comments.ToList());
        }

        // GET: Comments/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId, Description, DatePosted, IsApproved, PostId, UserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Title", comment.PostId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Title", comment.PostId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId, Description, DatePosted, IsApproved, PostId, UserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Title", comment.PostId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null) // Check if the provided ID is null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // Return a bad request status if the ID is null
            }
            Comment comment = db.Comments.Find(id);     // Attempt to find the comment with the provided ID
            // Check if the comment is not found
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);         // Return the view with the comment for confirmation of deletion
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Handles the deletion of a comment after confirmation
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);      // Find the comment with the provided ID
            db.Comments.Remove(comment);                 // Remove the comment from the database
            db.SaveChanges();                            // Save changes to the database
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            // Check if disposing is true
            if (disposing)
            {
                db.Dispose();          // Dispose of the database context
            }
            base.Dispose(disposing);         // Call the base class Dispose method
        }
    }
}
