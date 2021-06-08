using Microsoft.EntityFrameworkCore;
using SpendingsApi.Models;

namespace SpendingsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Spendings> Spendings { get; set; }
        public DbSet<CarApi.Models.Car> Car { get; set; }
        public DbSet<CostsApi.Models.Costs> Costs { get; set; }
        public DbSet<UserCars> UserCars { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}