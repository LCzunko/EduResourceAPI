using EduResourceAPI.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduResourceAPI.Data
{
    public class EduResourceDbContext : IdentityDbContext
    {
        public DbSet<Material> Materials { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        public EduResourceDbContext(DbContextOptions<EduResourceDbContext> options)
            : base(options)
        {
        }
    }
}