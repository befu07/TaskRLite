using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Principal;

namespace TaskRLite.Data
{
    public class TaskRContext : DbContext
    {
        public TaskRContext(DbContextOptions<TaskRContext> options) : base(options) { }
        public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TaskItem> TaskItems { get; set; }
        public virtual DbSet<ToDoList> ToDoLists { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Name=AppDb");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUserRole>()
            .HasData(
                new AppUserRole() { Id = 1, RoleName = "Admin" },
                new AppUserRole() { Id = 2, RoleName = "FreeUser" },
                new AppUserRole() { Id = 3, RoleName = "PremiumUser" }
                );
        }
        public override void Dispose()
        {
            SqliteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            base.Dispose();
        }
    }
}
