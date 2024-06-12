using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Principal;

namespace TaskRLite.Data
{
    public class TaskRContext : DbContext
    {
        public TaskRContext(DbContextOptions<TaskRContext> options) : base(options) { }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
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
