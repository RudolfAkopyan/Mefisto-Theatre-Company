using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Models
{
    public class MefistoDBContext : IdentityDbContext<User>
    {
        // DbSet properties for interacting with the database tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        // Constructor for MefistoDBContext
        public MefistoDBContext() : base("MefistoConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new Databaseinitializer());     // Set a custom database initializer to be executed when the database is created
        }
        // Factory method to create an instance of MefistoDBContext
        public static MefistoDBContext Create()
        {
            return new MefistoDBContext();
        }
    }
}