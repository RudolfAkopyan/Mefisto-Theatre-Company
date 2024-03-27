using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Models.ViewModels
{
    //Property To Edit Employee Details
    public class EditEmployeeViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string Country { get; set; }
        
        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

    
        [Display(Name = "Emloyment Status")]  //Property For Employment Status 
        [Required]
        public EmploymentStatus EmloymentStatus { get; set; }

        [Display(Name ="Suspended")]  //Property For Suspended Role 
        [Required]
        public bool IsSuspended { get; set; }
    }
}