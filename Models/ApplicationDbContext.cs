using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Authenication;

namespace TodoAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ToDoItemModel> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoItemModel>(entity =>
            {
                entity.Property(e => e.ItemName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ItemStatus).IsRequired().HasMaxLength(1);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
