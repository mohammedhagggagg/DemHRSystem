using Dem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dem.DAL.Context
{
    public class MVCProjectDbContext :IdentityDbContext<ApplicationUser>
    {
        public MVCProjectDbContext(DbContextOptions<MVCProjectDbContext> options):base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //  =>  optionsBuilder.UseSqlServer("Server = . ; Database = G3MVCDb ; Trusted_Connextion = True");

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
      
    }
}
