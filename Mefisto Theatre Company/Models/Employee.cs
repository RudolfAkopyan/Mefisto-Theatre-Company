using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Models
{
    public class Employee:User
    {
        [Display(Name = "Employment status")]                // Additional property for employment status with a display name
        public EmploymentStatus EmploymentStatus { get; set; }
        public List<Post> Posts { get; set; }                // Navigation property representing a collection of posts associated with an employee

    }
    public enum EmploymentStatus                            // Enumeration to represent employment status options
    {
        FullTime,
        PartTime
    }
}