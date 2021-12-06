using EduResourceAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduResourceAPI.Data
{
    public static class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider, IConfigurationSection initialAdmin)
        {
            using var context = new EduResourceDbContext(serviceProvider.GetRequiredService<DbContextOptions<EduResourceDbContext>>());

            if (context.Users.Any()) return;

            SeedIdentity(serviceProvider, initialAdmin);
            SeedEntities(context);
        }

        private static void SeedIdentity(IServiceProvider serviceProvider, IConfigurationSection initialAdmin)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var adminRoleTask = roleManager.CreateAsync(new IdentityRole("Admin"));
            adminRoleTask.Wait();
            if (!adminRoleTask.Result.Succeeded) throw new InvalidOperationException(string.Join('\n', adminRoleTask.Result.Errors));

            var userRoleTask = roleManager.CreateAsync(new IdentityRole("User"));
            userRoleTask.Wait();
            if (!userRoleTask.Result.Succeeded) throw new InvalidOperationException(string.Join('\n', userRoleTask.Result.Errors));

            var initialAdminUser = new IdentityUser() { Email = initialAdmin["Email"], UserName = initialAdmin["UserName"] };
            var adminUserTask = userManager.CreateAsync(initialAdminUser, initialAdmin["Password"]);
            adminUserTask.Wait();
            if (!adminUserTask.Result.Succeeded) throw new InvalidOperationException(string.Join('\n', adminUserTask.Result.Errors));

            var addInitialAdminToAdminRoleTask = userManager.AddToRoleAsync(initialAdminUser, "Admin");
            addInitialAdminToAdminRoleTask.Wait();
            if (!addInitialAdminToAdminRoleTask.Result.Succeeded) throw new InvalidOperationException(string.Join('\n', addInitialAdminToAdminRoleTask.Result.Errors));

            var addInitialAdminToUserRoleTask = userManager.AddToRoleAsync(initialAdminUser, "User");
            addInitialAdminToUserRoleTask.Wait();
            if (!addInitialAdminToUserRoleTask.Result.Succeeded) throw new InvalidOperationException(string.Join('\n', addInitialAdminToUserRoleTask.Result.Errors));
        }

        private static void SeedEntities(EduResourceDbContext context)
        {
            Author[] authors =
            {
                new Author
                {
                    Name = "Dominik Starzyk",
                    Description = "Motorola academy mentor, CodeCool employee."
                },
                new Author
                {
                    Name = "Lukasz Czunko",
                    Description = "Motorola academy student."
                },
                new Author
                {
                    Name = "Codecool Global",
                    Description = "international company providing programming courses which come with real-life team projects in mentor-led, online classes."
                }
            };

            context.AddRange(authors);

            Category[] categories =
            {
                new Category
                {
                    Name = "Documentation",
                    Definition = "Detailed information on key language features provided by an official source."
                },
                new Category
                {
                    Name = "Video",
                    Definition = "Video multimedia which transforms a passive learning experience into an active one."
                },
                new Category
                {
                    Name = "Literature",
                    Definition = "Traditionally published books on programming and other educational topics."
                },
                new Category
                {
                    Name = "Interactive",
                    Definition = "Interactive websites such as coding practice sites where you can learn various programming languages."
                }
            };

            context.AddRange(categories);

            Material[] materials =
            {
                new Material
                {
                    Title = "CodeWars",
                    Description = "Codewars is a coding practice site for all programmers where you can learn various programming languages. Join the community and improve your skills.",
                    Location = "https://www.codewars.com/",
                    Published = DateTime.Parse("2012-11-01"),
                    Author = authors[2],
                    Category = categories[3]
                },
                new Material
                {
                    Title = "Swagger in ASP.Net Core (Using Swashbuckle.AspNetCore NuGet Package)",
                    Description = "YT Tutorial on how to set up Swagger.",
                    Location = "https://www.youtube.com/watch?v=jg_e011SjzE",
                    Published = DateTime.Parse("2020-03-23"),
                    Author = authors[0],
                    Category = categories[1]
                },
                new Material
                {
                    Title = "Introduction to ASP.NET Core MVC in C# plus LOTS of Tips",
                    Description = "Intro to writing ASP .NET Core MVC app.",
                    Location = "https://www.youtube.com/watch?v=1ck9LIBxO14",
                    Published = DateTime.Parse("2020-06-08"),
                    Author = authors[0],
                    Category = categories[1]
                },
                new Material
                {
                    Title = "Get started with ASP.NET Core MVC",
                    Description = "Tutorial on how to write an app.",
                    Location = "https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc",
                    Published = DateTime.Parse("2021-11-08"),
                    Author = authors[0],
                    Category = categories[0]
                },
                new Material
                {
                    Title = "Model validation in ASP.NET Core MVC and Razor Pages",
                    Description = "How to validate a model state.",
                    Location = "https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation",
                    Published = DateTime.Parse("2021-10-05"),
                    Author = authors[0],
                    Category = categories[0]
                }
            };

            context.AddRange(materials);

            Review[] reviews =
            {
                new Review
                {
                    Text = "I found the explanation overly technical and unclear.",
                    Score = 3,
                    Material = materials[4]
                },
                new Review
                {
                    Text = "I understand validation better now, I guess...",
                    Score = 5,
                    Material = materials[4]
                },
                new Review
                {
                    Text = "I followed along with the tutorial and I learned a lot",
                    Score = 9,
                    Material = materials[3]
                }
            };

            context.AddRange(reviews);

            context.SaveChanges();
        }
    }
}
