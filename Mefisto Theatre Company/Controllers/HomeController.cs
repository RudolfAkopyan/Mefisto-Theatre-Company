using Mefisto_Theatre_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Mefisto_Theatre_Company.Models.ViewModels;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Controllers
{
    public class HomeController : Controller
    {
        private MefistoDBContext context = new MefistoDBContext(); 
        public ActionResult Index()       // Display the default view for the home page
        {
            return View();
        }
        public ActionResult AllPosts()
        {
            // Retrieve all posts with associated category and user details, ordered by date posted
            var posts = context.Posts.Include(p => p.Category).Include(p => p.User).OrderByDescending(p => p.DatePosted);
            ViewBag.Categories = context.Categories.ToList();
            return View(posts.ToList());            // Display the view with the list of posts and categories
        }
        // POST: Home/AllPosts
        [HttpPost]
        // Handle the search functionality based on category name
        public ViewResult AllPosts(string SearchString)
        {
            // Retrieve posts filtered by category name, including category and user details, ordered by date posted
            var posts = context.Posts.Include(p => p.Category).Include(p => p.User).Where(p => p.Category.Name.Equals(SearchString.Trim())).OrderByDescending(p => p.DatePosted);
            ViewBag.Categories = context.Categories.ToList();
            return View(posts.ToList());            // Display the view with the filtered list of posts and categories
        }
        // GET: Home/Details
        public ActionResult Details(int id)
        {
            // Retrieve post details by ID and foreign keys, including category, user, and associated comments with user details
            Post post = context.Posts
            .Include(p => p.Category)
            .Include(p => p.User)
            .Include(p => p.Comments.Select(c => c.User))
            .SingleOrDefault(p => p.PostId == id);

            // Send the post model to the details view
            return View(post);

        }
        // GET: Home/About
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";     // Display the about page view with a message

            return View();
        }
        // GET: Home/Contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";         // Display the contact page view with a message

            return View();
        }
    }
}