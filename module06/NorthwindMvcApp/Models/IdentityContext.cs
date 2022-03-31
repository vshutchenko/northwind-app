using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NorthwindMvcApp.Models
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string employeeRoleName = "employee";
            string customerRoleName = "customer";

            string adminName = "admin1";
            string adminPassword = "123456";

            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role employeeRole= new Role { Id = 2, Name = employeeRoleName };
            Role customerRole = new Role { Id = 3, Name = customerRoleName };
            User adminUser = new User { Id = 1, Name = adminName, PasswordHash = adminPassword, RoleId = adminRole.Id, NorthwindDbId = "-1" };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, employeeRole, customerRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });

            base.OnModelCreating(modelBuilder);
        }
    }
}
