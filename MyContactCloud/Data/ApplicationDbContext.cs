using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyContactCloud.Model;

namespace MyContactCloud.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public virtual DbSet<ImageUpload> Images { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Category> Categories { get; set; }

    }

}
