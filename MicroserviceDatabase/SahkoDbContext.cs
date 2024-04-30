using MicroserviceDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceDatabase
{
    public class SahkoDbContext : DbContext
    {
        public DbSet<ElectricityData> Prices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Encrypt=False;Trust Server Certificate=False;Command Timeout=0");
        }
    }


}
