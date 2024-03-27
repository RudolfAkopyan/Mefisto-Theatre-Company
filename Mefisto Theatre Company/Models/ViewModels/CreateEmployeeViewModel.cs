using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mefisto_Theatre_Company.Models.ViewModels;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Models.ViewModels
{
    //Property To new Create Employee 
    public class CreateEmployeeViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        [DataType(DataType.PostalCode)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Email Confirm")]
        public bool EmailConfirm { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Emloyment Status")] 
        public EmploymentStatus EmloymentStatus { get; set; }    //Property For Employment Status 

        public string Role { get; set;}
        public ICollection<SelectListItem> Roles { get; set;}   
    }
}