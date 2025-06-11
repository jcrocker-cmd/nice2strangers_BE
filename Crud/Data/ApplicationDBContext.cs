using Crud.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data
{
    //It acts as a bridge between your C# application and the actual database.
    public class ApplicationDBContext : DbContext
    {
        //Contructor
        //- The constructor takes DbContextOptions<ApplicationDBContext> as a parameter, which helps configure the context (e.g., setting up the connection string)
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        //DbSet from Microsoft.EntityFrameworkCore
        //<Employee> from Model/Entity
        //Employess represents table toa allow CRUD operations
        public DbSet<Employee> Employees{ get; set; }
        public DbSet<SP_Employee> SP_Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
