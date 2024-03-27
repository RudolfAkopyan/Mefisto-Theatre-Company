using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mefisto_Theatre_Company.Models.ViewModels;
using System.Web.Services.Description;

//30343322 Rudolf Akopyan
namespace Mefisto_Theatre_Company.Models
{
    public class Databaseinitializer : DropCreateDatabaseAlways<MefistoDBContext>
    {
        protected override void Seed(MefistoDBContext context)
        {
            {
                base.Seed(context);
                if (!context.Users.Any())
                {
                    // Check if there are no users in the database
                    RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    // Create and add roles if they don't exist
                    if (!roleManager.RoleExists("Admin"))
                    {
                        roleManager.Create(new IdentityRole("Admin"));
                    }
                    if (!roleManager.RoleExists("Moderator"))
                    {
                        roleManager.Create(new IdentityRole("Moderator"));
                    }
                    if (!roleManager.RoleExists("Customer"))
                    {
                        roleManager.Create(new IdentityRole("Customer"));
                    }
                    if (!roleManager.RoleExists("Member"))
                    {
                        roleManager.Create(new IdentityRole("Member"));
                    }
                    if (!roleManager.RoleExists("Suspended"))
                    {
                        roleManager.Create(new IdentityRole("Suspended"));
                    }
                    context.SaveChanges();       // Save changes to the context
                }
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));        // Create a UserManager to manage users
                userManager.PasswordValidator = new PasswordValidator()                                     // Set up a simple password validator for users
                {
                    RequiredLength = 1,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                };
                var admin = new Employee()
                {
                    UserName = "admin@mafiesto.com",         // Create a new Employee
                    Email = "admin@mafiesto.com",
                    FirstName = "Genry",
                    LastName = "Ford",
                    Address1 = "11 Hope Street",
                    Address2 = "189",
                    City = "Glasgow",
                    Country = "Scotalnd",
                    PostCode = "G12 2HD",
                    RegesteredAt = DateTime.Now.AddYears(-1),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false,
                    EmploymentStatus = EmploymentStatus.FullTime
                };
                if (userManager.FindByName("admin@mafiesto.com") == null)       // Check if a Employee with the username "admin@mafiesto.com" exists
                {
                    // If not, create the user and assign a role
                    userManager.Create(admin, "admin123");
                    userManager.AddToRole(admin.Id, "Admin");
                }

                var anton = new Employee()              // Create a new Employee
                {
                    UserName = "anton@mafiesto.com",
                    Email = "anton@mafesto.com",
                    FirstName = "Anton",
                    LastName = "Petrov",
                    Address1 = "2  Argyle street",
                    Address2 = "2/2",
                    City = "Glasgow",
                    Country = "Scotalnd",
                    PostCode = "G13 3DS",
                    RegesteredAt = DateTime.Now.AddDays(-200),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false,
                    EmploymentStatus = EmploymentStatus.FullTime
                };
                if (userManager.FindByName("anton@mafiesto.com") == null)       // Check if a Employee with the username "anton@mafiesto.com" exists
                {
                    // If not, create the user and assign a role
                    userManager.Create(anton, "moderator");
                    userManager.AddToRole(anton.Id, "Moderator");
                }

                    var paul = new Customer()       // Create a new Customer
                    {
                        UserName = "paul@gmail.com",
                        Email = "paul@gmail.com",
                        FirstName = "Paul",
                        LastName = "Golf",
                        Address1 = "30 Drumry",
                        Address2 = "122",
                        City = "Glasgow",
                        Country = "Scotalnd",
                        PostCode = "G11 6QW",
                        RegesteredAt = DateTime.Now.AddDays(-30),
                        EmailConfirmed = true,
                        IsActive = true,
                        IsSuspended = false,
                        CustomerType = CustomerType.Customer
                    };
                    if (userManager.FindByName("paul@gmail.com") == null)   // Check if a user with the username "paul@mafiesto.com" exists
                {
                        // If not, create the user and assign a role
                        userManager.Create(paul, "customer");
                        userManager.AddToRole(paul.Id, "Customer");
                    }                
                var alex = new Customer()    // Create a new Customer
                {
                    UserName = "alex@gmail.com",
                    Email = "alex@gmail.com",
                    FirstName = "alex",
                    LastName = "Kater",
                    Address1 = "233 Buchanan",
                    Address2 = "0/2",
                    City = "Glasgow",
                    Country = "Scotalnd",
                    PostCode = "G11 3SA",
                    RegesteredAt = DateTime.Now.AddDays(-5),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = true,
                    CustomerType = CustomerType.VipCustomer
                };
                if (userManager.FindByName("alex@gmail.com") == null)           // Check if a user with the username "alex@gmail.com" exists
                {
                    // If not, create the user and assign a role
                    userManager.Create(alex, "member");
                    userManager.AddToRole(alex.Id, "Member");
                }
                var sasha = new Customer()          // Create a new Customer 
                {
                    UserName = "sasha@gmail.com",
                    Email = "sasha@gmail.com",
                    FirstName = "Sasha",
                    LastName = "Poezd",
                    Address1 = "12 Hummer Street",
                    Address2 = "0/8",
                    City = "Glasgow",
                    Country = "Scotalnd",
                    PostCode = "G61 3AA",
                    RegesteredAt = DateTime.Now.AddDays(-5),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = true,
                    CustomerType = CustomerType.Customer
                };
                if (userManager.FindByName("sasha@gmail.com") == null)      // Check if a user with the username "sasha@gmail.com" exists
                {
                    // If not, create the user and assign a role
                    userManager.Create(sasha, "password3");
                    userManager.AddToRole(sasha.Id, "Member");

                }
                context.SaveChanges();
               // Create Category objects
                var cat1 = new Category() { Name = "Theatre Soon" };
                var cat2 = new Category() { Name = "Reviews" };
                var cat3 = new Category() { Name = "Blogs" };


                context.Categories.Add(cat1);
                context.Categories.Add(cat2);
                context.Categories.Add(cat3);

                context.SaveChanges();

                var Post1 = new Post()
                {
                    Title = "Romeo and Juliet",     // Set the title of the post
                                                    // Set the description of the post, providing details about the musical
                    Description = "Musical in 2 actsMusic - Arkady Ukupnik Libretto and lyrics - Karen Kavaleryan Stage director - Alexey Frandetti Ballet master - Irina Kashuba Design - Vyacheslav Okunev Musical director - Konstantin Khvatynets Lighting designers - Alexander Sivaev Choirmaster - Stanislav Maisky FANTASY WITH CHILDREN'S ELEMENTS TIVAMusical “Romeo VS Juliet. XX years later\" was specially written by composer Arkady Ukupnik and playwright Karen Kavaleryan for the Moscow Operetta Theater. The plot of the musical is based on the assumption of what could have happened to the heroes two decades later if the ending of the great tragedy had been different and its heroes had remained alive. This is practically a sequel to \"Romeo and Juliet\", a dramatic and musical fantasy with detective elements on the themes of Shakespeare's most famous play. In the original production, unexpected and unpredictable plot twists are intertwined with funny comedic scenes that create the atmosphere of that time, and the music develops and emotionally colors the sublime. romantic and dramatic events, over which the image of Shakespeare himself reigns as the ruler of souls and passions.",
                    DatePosted = new DateTime(2023, 1, 1, 8, 0, 15),    // Set the date and time when the post was originally posted
                    DateEdited = new DateTime(2023, 1, 1, 8, 0, 15),     // Set the date and time when the post was last edited (initially set to the same as DatePosted)
                    User = anton,                                         // Set the user who posted the content (presumably an instance of the 'anton' user)
                    Category = cat1,                                        // Set the category associated with the post (presumably an instance of 'cat1')
                };
                context.Posts.Add(Post1);

                var Post2 = new Post()
                {
                    Title = "Anna Karenina",
                    Description = "\"Anna Karenina\" - A WORLD MASTERPIECE IN ONE BREATH Producers - Vladimir Tartakovsky, Alexey Bolonin Author of the libretto - Yuliy Kim Composer - Roman Ignatiev Stage director - Alina Chevik Choreographer - Irina Korneeva Musical director - Konstantin Khvatynets Production designer - Vyacheslav Okunev Lighting Designer – Gleb FilshtinskyMakeup and Design Artist hairstyles - Andrey Drykin The musical “Anna Karenina” won millions of hearts around the world and earned recognition from many theater critics. Since the premiere, more than 800 performances have been performed in Russia and abroad. The production’s troupe includes the stars of the capital’s musical: Ekaterina Guseva, Valeria Lanskaya, Dmitry Ermak, Sergey Li, Olga Belyaeva, Natalia Bystrova, Igor Balalaev, Lika Rulla, Andrey Birin, Alexander Marakulin, Maxim Zausalin and many others. The heroes of the musical experience vivid and contradictory feelings: love and betrayal, passion and duty, hope and despair.",
                    DatePosted = new DateTime(2022, 3, 2, 10, 0, 15),
                    DateEdited = new DateTime(2023, 1, 1, 8, 0, 15),
                    User = anton,
                    Category = cat1,
                };
                context.Posts.Add(Post2);

                var Post3 = new Post()
                {
                    Title = "Monte Cristo",
                    Description = "\"MONTE CRISTO\": A LEGENDARY HISTORY Producers - Vladimir Tartakovsky, Alexey Bolonin Author of the libretto - Yuliy Kim Composer - Roman Ignatiev Stage director - Alina Chevik Choreographer - Irina Korneeva Production designer - Vyacheslav Okunev Lighting designer - Gleb FilshtinskyArtist makeup and hairstyle: Andrey Drykin \"Monte\" -Christo\" is the first completely original Russian musical created on the basis of a world classic work. Since its premiere, the hit musical has won the hearts of millions of viewers and received a number of prestigious theater awards, including on the Asian continent. Since the premiere, more than 700 performances have been performed in Russia and abroad. The latest sound and lighting equipment, a unique projection screen and modern technical equipment of the theater puts “Monte Cristo” on a par with the most expensive world productions. The stage space turns into a ballroom, the mysterious Château d'If and even the sails of an entire armada of smugglers' ships. Incredible voices of actors, bright costumes and scenery, modern acrobatic stunts and various special effects are intertwined into a bright, exciting action with an amazingly beautiful musical score. It either takes the listener back to the depths of centuries, or suddenly, thanks to its incredible energy, reminds us of today's reality.",
                    DatePosted = new DateTime(2023, 1, 1, 8, 0, 15),
                    DateEdited = new DateTime(2023, 1, 1, 8, 0, 15),
                    User = anton,
                    Category = cat1,
                };
                context.Posts.Add(Post3);

                var Post4 = new Post()
                {
                    Title = "Cinderela",
                    Description = "The fairy tale about the hardworking beauty Cinderella, her evil stepmother, lazy sisters and a fairy-tale prince has been played in front of young spectators more than three hundred times! At the conductor's stand, as at the premiere, is composer and conductor Andrei Semyonov. In general, as the gatekeepers of the fairy-tale kingdom say: “Come, don’t forget! Honestly, you won’t regret it!” The roles include recognized masters of the genre and young artists: People’s Artists of Russia Svetlana Varguzova and Elena Ionova; Olga Belokhvostova, Vasilisa Nikolaeva, Anastasia Pugina, Honored Artist of Russia Pyotr Borisenko, Dmitry Nikanorov, Alexander Frolov and others.",
                    DatePosted = new DateTime(2002, 5, 8, 3, 0, 15),
                    DateEdited = new DateTime(2023, 1, 1, 8, 0, 15),
                    User = anton,
                    Category = cat3,
                };
                context.Posts.Add(Post4);

                var Post5 = new Post()
                {
                    Title = "King Arthur",
                    Description = "The fairy tale about the hardworking beauty Cinderella, her evil stepmother, lazy sisters and a fairy-tale prince has been played in front of young spectators more than three hundred times! At the conductor's stand, as at the premiere, is composer and conductor Andrei Semyonov. In general, as the gatekeepers of the fairy-tale kingdom say: “Come, don’t forget! Honestly, you won’t regret it!” The roles include recognized masters of the genre and young artists: People’s Artists of Russia Svetlana Varguzova and Elena Ionova; Olga Belokhvostova, Vasilisa Nikolaeva, Anastasia Pugina, Honored Artist of Russia Pyotr Borisenko, Dmitry Nikanorov, Alexander Frolov and others.",
                    DatePosted = new DateTime(2018, 8, 2, 8, 0, 15),
                    DateEdited = new DateTime(2023, 1, 1, 8, 0, 15),
                    User = anton,
                    Category = cat2,
                };
                context.Posts.Add(Post5);
                var Post6 = new Post()
                {
                    Title = "King Arthur",
                    Description = "The story of King",
                    DatePosted = new DateTime(2018, 8, 2, 8, 0, 15),
                    DateEdited = new DateTime(2023, 1, 1, 8, 0, 15),
                    User = anton,
                    Category = cat2,
                };
                context.Posts.Add(Post6);
                var Post7 = new Post()
                {
                    Title = "King Arthur",
                    Description = "New Story",
                    DatePosted = new DateTime(2018, 8, 2, 8, 0, 15),
                    DateEdited = new DateTime(2028, 1, 1, 8, 0, 15),
                    User = admin,
                    Category = cat3,
                };
                context.Posts.Add(Post7);
                context.SaveChanges();

                var comment1 = new Comment()
                {
                    Description = "This Story is Really Amazing!",           // Set the description of the comment
                    DatePosted = new DateTime(2018, 8, 3, 9, 0, 15),         // Set the date when the comment was posted
                    Post = Post4,                                            // Set the associated Post for the comment
                    User = sasha,                                            // Set the associated User (presumably an instance of the 'sasha' user)
                };
                context.Comments.Add(comment1);           
                var comment2 = new Comment()
                {
                    Description = "I don't like this at all :( ",
                    DatePosted = new DateTime(2023, 6, 1, 9, 0, 15),
                    Post = Post1,
                    User = alex,
                };
                context.Comments.Add(comment2);
                var comment3 = new Comment()
                {
                    Description = "Cool (^__^)",
                    DatePosted = new DateTime(2025, 4, 1, 9, 0, 15),
                    Post = Post2,
                    User = paul,
                };
                context.Comments.Add(comment3);
                context.SaveChanges();
            } //end if 
        } //end seed method
    } //end class 
} //end namescpace 

