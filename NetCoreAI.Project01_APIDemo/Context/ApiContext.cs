using Microsoft.EntityFrameworkCore;
using NetCoreAI.Project01_APIDemo.Entities;

namespace NetCoreAI.Project01_APIDemo.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=BEYZA\\BEYZA_DEV; Initial catalog=ApiAIDb; Integrated Security=true; Trusted_Connection=True; TrustServerCertificate=True;");
        }

        public DbSet<Customer>Customers { get; set; }
    }
}
