using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Models
{
    public class Category
    {
        // Primary key for the Category entity
        [Key]
        public int CategoryId { get; set; }
        // Display name for the category in the UI.
        [Display(Name = "Category")]
        public string Name { get; set; }
        // Indicates that a category can have multiple posts associated with it.
        public List<Post> Posts { get; set;}
    }
}