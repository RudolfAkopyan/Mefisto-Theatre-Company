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
    [Authorize(Roles = "Moderator")]
    public class ModeratorController : Controller
    {
        private MefistoDBContext db = new MefistoDBContext();
        // GET: Moderator
        [Authorize(Roles = "Moderator")]
        public ActionResult Index()      // Display the Moderator's index view
        {
            return View();
        }

        //GET: Categories
        public ActionResult ViewAllCategories()
        {
            //return the ViewAllCategories view that will display a list of categories
            return View(db.Categories.ToList());
        }

        //GET: Categories/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find category by id in Categories in table in database
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            //send the category to th Details view
            return View(category);
        }



        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();  // Display the create category view
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId, Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                // Add the category to the database and save changes
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("ViewAllCategories");        // Redirect to the ViewAllCategories action
            }
            return View(category);

        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find category by id in Categories in table in database
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);      // Display the edit category view
        }

        // POST: Moderator/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handle category editing
        public ActionResult Edit([Bind(Include = "CategoryId, Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                // Modify the category state and save changes
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAllCategories");    // Redirect to the ViewAllCategories action
            }
            return View(category);
        }

        // GET: Moderator/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //find category by id in Categories in table in database
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);      // Display the delete category view
        }

        // POST: Moderator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Handle category deletion
        public ActionResult DeleteConfirmed(int id)
        {
            // Find category by id in Categories table in the database
            Category category = db.Categories.Find(id);
            // Remove the category and save changes
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("ViewAllCategories");       // Redirect to the ViewAllCategories action

        }
        // Dispose method to release resources
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Moderator")]
        // View all posts including their category and user who created the post
        public ActionResult ViewAllPosts()
        {
            //get all posts from database including their category and user who created post
            List<Post> posts = db.Posts.Include(p => p.Category).Include(p => p.User).ToList();
            //send the list to the view named ViewAllPosts
            return View(posts);
        }

        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        // Handle post deletion
        public ActionResult DeletePostConfirmed(int id)
        {
            // Find post by id in Posts table in the database
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);      // Remove the post and save changes
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts");         // Redirect to the ViewAllPosts action
        }


        // GET: Categories/Edit/5
        public ActionResult EditPosts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find category by id in Categories in table in database
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            return View(post);       // Display the edit post view
        }

        // POST: Moderator/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handle post editing
        public ActionResult EditPosts(Post post)
        {
            if (ModelState.IsValid)
            {
                // Keep the original post date and modify the post state
                post.DatePosted = db.Posts.Find(post.PostId).DatePosted;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();                               //saves the changes into database
                return RedirectToAction("ViewAllPosts");        // Redirect to the ViewAllPosts action
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            return View(post);
        }

        public ActionResult DetailsPosts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find category by id in Categories in table in database
            Post post = db.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(p => p.Comments.Select(c => c.User))
                .SingleOrDefault(p => p.PostId == id);

            if (post == null)
            {
                return HttpNotFound();
            }
            //send the category to th Details view
            return View(post);
        }

        //Moderator can view all comments

        public ActionResult ViewAllComments()
        {
            // Get all comments from the database including their post and user who created the comment
            List<Comment> comments = db.Comments.Include(p => p.Post).Include(p => p.User).ToList();
            return View(db.Comments.ToList());       // Send the list to the view named ViewAllComments
        }

        [HttpPost]
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Find comment by id in Comments table in the database
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
           // return View(comment);
            db.Comments.Remove(comment);                        // Remove the comment and save changes
            db.SaveChanges();
            return RedirectToAction("ViewAllComments");
            
        }

        //[HttpPost, ActionName("DeleteComment")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteCommentConfirmed(int id)
        //{
        //    Comment comment = db.Comments.Find(id);              // Find comment by id in Comments table in the database
        //    db.Comments.Remove(comment);                        // Remove the comment and save changes
        //    db.SaveChanges();
        //    return RedirectToAction("ViewAllComments");          // Redirect to the ViewAllComments action
        //}
    }
}
