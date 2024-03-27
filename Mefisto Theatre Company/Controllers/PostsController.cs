using Mefisto_Theatre_Company.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Controllers
{
    public class PostsController : Controller
    {
        private MefistoDBContext db = new MefistoDBContext();

        // GET: Posts
        public ActionResult Index()     // Display a list of posts including their category and user who created the post
        {
            var posts = db.Posts.Include(p => p.Category).Include(p => p.User);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)        // Display details of a specific post
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)                       //checks if Post exist
            {
                return HttpNotFound();              
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()                // Display the create post view with category and user options
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handle post creation
        public ActionResult Create([Bind(Include = "PostId,Title,Description,Location,DatePosted,DateEdited,CategoryId,UserId")] Post post)         // Handles the creation of a new post
        {
            if (ModelState.IsValid)         // Check if the model state is valid(i.e., if the data entered is valid)
            {
                db.Posts.Add(post);         // Add the post to the database and save changes
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // If the model state is not valid, reload the view with the posted data
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", post.UserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        // Displays the view for editing a specific post
        [Authorize(Roles = "Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)     // Check if the provided ID is null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);      // Find the post in the database based on the provided ID
            if (post == null)
            {
                return HttpNotFound();          // Return a not found status if the post is not found
            }
            // Load the view with the post data and associated categories and users
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", post.UserId);
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handles the submission of the edited post
        public ActionResult Edit([Bind(Include = "PostId, Title, Description, DatePosted, DateEdited, CategoryId, UserId")] Post post)
        {
            // Check if the model state is valid (if the data entered is valid)
            if (ModelState.IsValid)
            {
                // Update the post in the database and save changes
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");       // Redirect to the Index action to display the updated list of posts
            }
            // If the model state is not valid, reload the view with the posted data
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", post.UserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id) // Displays the view for confirming the deletion of a specific post
        {
            if (id == null)     // Check if the provided ID is null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);  // Return a bad request status if the ID is null
            }
            Post post = db.Posts.Find(id);   // Find the post in the database based on the provided ID
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);          // Load the view with the post data for confirmation
        }

        // POST: Posts/Delete/5
        // Handles the deletion of a specific post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);      // Find the post in the database based on the provided ID
            db.Posts.Remove(post);              // Remove the post from the database and save changes
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();       // Dispose of the database context
            }
            base.Dispose(disposing);
        }
    }
}
