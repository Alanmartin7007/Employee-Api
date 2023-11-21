using Microsoft.EntityFrameworkCore;

namespace EmsApi.Models
{
    public class EmsContext : DbContext
    {
        public EmsContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
