using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Migrations;
using LM.Api.Models;
using LM.Api.Models.DB;
using LM.Api.Models.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LM.Api.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<LibraryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true; 
            AutomaticMigrationDataLossAllowed = true; 

            CommandTimeout = 3600;
            ContextKey = " LM.Api.Models.LibraryDbContext";

            //logging
            DbInterception.Add(new EFInterceptor());
        }

        protected override void Seed(LibraryDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var userManager = new UserManager<User, string>(new UserStore<User>(context));
            var user = userManager.FindByEmail("elyor.blog@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    Email = "elyor.blog@gmail.com",
                    UserName = "elyor.blog@gmail.com",
                    FirstName = "Elyor",
                    LastName = "Latipov",
                    EmailConfirmed = true
                };
                userManager.Create(user, "LeO_052989");
            }

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Books.AddOrUpdate(p => p.Id, new Book
            {
                Id = 1,
                CountOfPage = 145,
                ISBN = "99921-58-10-7",
                PicturePath = "avatar.png",
                Publisher = "NCCAH, Doha",
                Title = "Qatar",
                Year = 1979,
                Authors = new List<Author>
                {
                    new Author {FirstName = "A1", LastName = "B1"},
                    new Author {FirstName = "A2", LastName = "B2"},
                    new Author {FirstName = "A3", LastName = "B3"},
                    new Author {FirstName = "A4", LastName = "B4"},
                }
            });

            context.SaveChanges();
        }
    }
}