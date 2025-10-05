using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Crud.Models.Entities.Services;
using Crud.Models.Entities;
using Crud.Models.Auth;

namespace Crud.Data
{
    //It acts as a bridge between your C# application and the actual database.
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        //Contructor
        //- The constructor takes DbContextOptions<ApplicationDBContext> as a parameter, which helps configure the context (e.g., setting up the connection string)
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // <-- Important: call base for Identity tables
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            // ...any other configuration
        }
        //DbSet from Microsoft.EntityFrameworkCore
        //<Employee> from Model/Entity
        //Employess represents table toa allow CRUD operations
        public DbSet<Employee> Employees{ get; set; }
        public DbSet<ContactUs> ContactUs{ get; set; }
        public DbSet<Newsletter> Newsletter { get; set; }
        public DbSet<SP_Employee> SP_Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<VideoEditingModel> VideoEditingModel { get; set; }
        public DbSet<DroneModel> DroneModel { get; set; }
        public DbSet<SocialMediaConsultingModel> SocialMediaConsultingModel { get; set; }
        public DbSet<SocialMediaCreationModel> SocialMediaCreationModel { get; set; }
        public DbSet<WebAppModel> WebAppModel { get; set; }
    }
}
