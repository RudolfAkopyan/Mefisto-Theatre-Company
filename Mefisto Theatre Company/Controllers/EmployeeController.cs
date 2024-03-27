using Mefisto_Theatre_Company.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Controllers
{
    public class EmployeeController : Controller
    {
            // GET: Employee
            private MefistoDBContext db = new MefistoDBContext();

            
            [Authorize(Roles = "Admin, Moderator, Staff")] // Only staff members can access this action to view their posts
            public ActionResult Index()
            {
                // Retrieve posts for the currently logged-in staff member
                var posts = db.Posts.Include(p => p.Category).Include(p => p.User);
                var userId = User.Identity.GetUserId();
                posts = posts.Where(p => p.UserId == userId);
                return View(posts.ToList());
            }

            // GET: Staff/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // Return a bad request status if the ID is null
            }
            // Retrieve details of a specific post including its category, user, and associated comments
             Post post = db.Posts
            .Include(p => p.Category)
            .Include(p => p.User)
            .Include(p => p.Comments.Select(c => c.User))
            .SingleOrDefault(p => p.PostId == id);

            //if post 
            if (post == null)
            {
                return HttpNotFound();      // Return a not found status if the post is not found
            }
            return View(post);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }
            // POST: Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handle the creation of a new post
        public ActionResult Create([Bind(Include = "PostId, Title, Description, CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                // Set the date posted, user ID, add the post to the database, and save changes
                post.DatePosted = DateTime.Now;
                post.UserId = User.Identity.GetUserId();
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            return View(post);
        }
            // GET: Staff/Edit/5
            public ActionResult Edit(int? id)
            {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);     // Return a bad request status if the ID is null
            }
            // Retrieve the post for editing
            Post post = db.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();      // Return a not found status if the post is not found
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);

            return View(post);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handle the editing of a post
        public ActionResult Edit([Bind(Include = "PostId, Title, Content,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                // Set the date posted, user ID, update the post details, and save changes
                post.DatePosted = DateTime.Now;
                post.UserId = User.Identity.GetUserId();
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            return View(post);

        }

        // GET: Staff/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);       // Return a bad request status if the ID is null
            }
            // Retrieve the post for deletion with associated category details
            Post post = db.Posts.Find(id);
            var category = db.Categories.Find(post.CategoryId);
            post.Category = category;

            if (post == null)
            {
                return HttpNotFound();      // Return a not found status if the post is not found
            }
            return View(post);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)            // Handle the deletion of a post after confirmation
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
    
