using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Models.ViewModels
{
    public class ChangeRoleViewModel //Change Employee Role
    {
        //Property to change role 
        public string UserName { get; set; }
        public string OldRole { get; set; }
        public ICollection<SelectListItem> Roles{ get; set; }
       
        [Required, Display(Name = "Role")]
        public string Role { get; set; }
    }
}