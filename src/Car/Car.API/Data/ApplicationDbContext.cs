using CarApi.Models;
using Microsoft.EntityFrameworkCore;


namespace CarApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Car> Car { get; set; }
        public DbSet<UserCars> UserCars { get; set; }

    }
}
