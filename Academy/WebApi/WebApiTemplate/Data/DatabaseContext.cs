using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApiTemplate.Data.Models;
using WebApiTemplate.Models;

namespace WebApiTemplate.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() {}
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Database=Academy;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
        }

        public virtual DbSet<Car> Cars { get; set; }   
    }
}
