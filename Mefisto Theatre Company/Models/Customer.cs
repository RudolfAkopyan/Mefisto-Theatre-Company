using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Models
{
    public class Customer : User
    {
        // Additional properties for customers
        [Display(Name = "Customer type")]
        public CustomerType CustomerType { get; set; }

    }
    public enum CustomerType //different types of customers
    {
        Customer,
        VipCustomer
    }
}