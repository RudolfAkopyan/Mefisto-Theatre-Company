using Mefisto_Theatre_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using Mefisto_Theatre_Company.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using static Mefisto_Theatre_Company.ApplicationUserManager;
using System.Net;
using Microsoft.Owin.BuilderProperties;
using System.Collections;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : AccountController
    {
        // Database context for accessing user data
        private MefistoDBContext db = new MefistoDBContext();
        // Constructors
        public AdminController() : base()
        {

        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
            : base(userManager, signInManager)
        {

        }
        // GET: Admin
        // Displays a list of users ordered by registration date
        public ActionResult Index()
        {
            var users = db.Users.OrderBy(u => u.RegesteredAt).ToList();
            return View(users);
        }

        [HttpGet]
        // Displays the form for creating a new employee
        public ActionResult CreateEmployee()
        {
            // Create a view model for creating a new employee
            CreateEmployeeViewModel employee = new CreateEmployeeViewModel();
            // Fetch roles from the database and provide them to the view
            var roles = db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            employee.Roles = roles;

            return View(employee);
        }


        [HttpPost]
        // Handles the form submission for creating a new employee
        public ActionResult CreateEmployee(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee newEmployee = new Employee
                {
                    // Create a new Employee object and populate its properties
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    Country = model.Country,
                    PostCode = model.PostCode,
                    RegesteredAt = DateTime.Now,
                    IsActive = true,
                    IsSuspended = false,
                    EmploymentStatus = model.EmloymentStatus
                };
                
                var result = UserManager.Create(newEmployee, model.Password);
                // Check if user creation was successful
                if (result.Succeeded)
                {
                    UserManager.AddToRole(newEmployee.Id, model.Role);
                    return RedirectToAction("Index", "Admin");
                }
            }
            // If the model is not valid, reload the roles and show the form with validation errors
            var roles = db.Roles.Select(r => new SelectListItem 
            { 
                Text = r.Name,
                Value = r.Name
            }).ToList();
            model.Roles = roles;
            // Return the view with the model to display validation errors
            return View(model);
        }
        // Displays the form for editing staff details
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditStaff(string id)
        {
            // Check if the provided ID is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Attempt to find the user with the provided ID and cast it to an Employee
            Employee staff = db.Users.Find(id) as Employee;
            // Check if the employee is not found
            if (staff == null)
            {
                return HttpNotFound();
            }

            return View(new EditEmployeeViewModel
            {
                // Return the view with an EditEmployeeViewModel containing the employee's details
                Address1 = staff.Address1,
                Address2 = staff.Address2,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                City = staff.City,
                Country = staff.Country,
                PostCode = staff.PostCode,
                EmloymentStatus = staff.EmploymentStatus,
                IsSuspended=staff.IsSuspended
            });
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handles the form submission for editing staff details
        public async Task<ActionResult> EditStaff(string id,
            [Bind(Include = "FirstName, LastName, Address1, Address2, City, Country, PostCode, EmloymentStatus, IsSuspended")]EditEmployeeViewModel model)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
            Employee staff = (Employee)await UserManager.FindByIdAsync(id);

                UpdateModel(staff);
                IdentityResult result = await UserManager.UpdateAsync(staff);
                // Check if the update was successful
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            // If the model state is not valid, return the view with the model to display validation errors
            return View(model);
        }
        [HttpGet]
        public ActionResult EditCustomer(string id) 
        {
            // Check if the provided ID is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Attempt to find the user with the provided ID and cast it to a Customer
            Customer customer = db.Users.Find(id) as Customer;
            // Check if the customer is not found
            if (customer == null)
            {
                return HttpNotFound();
            }
            // Return the view with an EditCustomerViewModel containing the customer's details
            return View(new EditCustomerViewModel
            {
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                City = customer.City,
                Country = customer.Country,
                PostCode = customer.PostCode,
                CustomerType = customer.CustomerType,
                IsSuspended = customer.IsSuspended
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handles the form submission for editing customer details
        public async Task<ActionResult> EditCustomer(string id, [Bind(Include = "FirstName, LastName, Address1, Address2, City, Country, PostCode, CustomerType, IsSuspended")] EditCustomerViewModel model)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Retrieve the customer by their ID
                Customer customer = (Customer)await UserManager.FindByIdAsync(id);
                // Update the customer's properties with the model's values
                UpdateModel(customer);
                IdentityResult result = await UserManager.UpdateAsync(customer);
                // Check if the update was successful
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View(model);
        }
        public ActionResult Details(string id)
        {
            // Check if the provided ID is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            // Check if the user is not found
            if (user == null)
            {
                return HttpNotFound();
            }
            // Display details based on the user type (Employee or Customer)
            if (user is Employee)
            {
                return View("DetailsStaff", (Employee)user);
            }
            if (user is Customer) 
            {
                return View("DetailsCustomer", (Customer)user);
            }
            // Return a not found status if the user type is unknown
            return HttpNotFound();
        }

        [HttpGet]
        // Displays the form for creating a new role
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        // Handles the form submission for creating a new role
        public ActionResult CreateRole(RoleViewModel model)
        {
        if (ModelState.IsValid)
                {
                // Create a RoleManager with the current database context
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                if (!roleManager.RoleExists(model.RoleName)) // Check if the role with the provided name already exists
                {
                    roleManager.Create(new IdentityRole { Name = model.RoleName });  // Create a new role with the provided name

                    return RedirectToAction("Index", "Admin");
                    }
                }
                    return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> ChangeRole(string id)
        {
            if (id == null)
            // Check if the provided ID is null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);  // Return a bad request status if the ID is null
            }
            if (id == User.Identity.GetUserId())    // Check if the provided ID is the same as the currently logged-in user's ID
            {
                return RedirectToAction("Index", "Admin");
            }
            User user = await UserManager.FindByIdAsync(id);  // Retrieve the user by the provided ID asynchronously
            string oldRole = (await UserManager.GetRolesAsync(id)).Single();
            
            // If the model is not valid, reload the roles and show the form with validation errors
            var items = db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = r.Name == oldRole
            }).ToList();

            return View(new ChangeRoleViewModel          // Return the view with a ChangeRoleViewModel containing the user's details and available roles
            {
                UserName = user.UserName,
                Roles = items,
                OldRole = oldRole,
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangeRole")]
        // Handles the form submission for changing a user's role
        public async Task<ActionResult> ChangeRoleConfirmed(string id, [Bind(Include ="Role")] ChangeRoleViewModel model)
        {
            User user = await UserManager.FindByIdAsync(id);
            // Get the user's previous role
            string preveousRole = (await UserManager.GetRolesAsync(id)).Single();
            // Check if the current user is attempting to change their own role
            if (id == User.Identity?.GetUserId()) 
            {
                return RedirectToAction("Index", "Admin");
            }
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                 user = await UserManager.FindByNameAsync(id);
                 preveousRole = (await UserManager.GetRolesAsync(id))?.Single();
                // Check if the new role is different from the previous role
                if (preveousRole != model.Role) 
                {
                    return RedirectToAction("Index", "Admin");
                }
                // Remove the user from their previous role and add them to the new role
                await UserManager.RemoveFromRoleAsync(id, preveousRole);
                await UserManager.AddToRoleAsync(id, model.Role);
                // If the new role is "Suspended," set the user's "IsSuspended" property to true
                if (model.Role == "Suspended") 
                {
                    user.IsSuspended = true;
                    await UserManager.UpdateAsync(user);
                }
                return RedirectToAction("Index", "Admin");
            }
            return View(model);
        }
        // Handles the request to delete a user
        public async Task<ActionResult> Delete(string id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Check if the current user is attempting to delete their own account
            if (User.Identity?.GetUserId() == id) 
            {
                return RedirectToAction("Delete", "Admin");
            }
            User user = await UserManager.FindByIdAsync(id);
            // Check if the user is not found
            if(user == null) 
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "Admin");
        }
        //!!
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Handle category deletion
        public ActionResult DeleteConfirmed(string id)
        {
            // Find category by id in Categories table in the database
            User user = db.Users.Find(id);
            // Remove the category and save changes
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");       // Redirect to the ViewAllCategories action

        }
        //!!!
        // Dispose method to release resources, including the database context
        protected override void Dispose(bool disposing) 
        {
            if(disposing) 
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}