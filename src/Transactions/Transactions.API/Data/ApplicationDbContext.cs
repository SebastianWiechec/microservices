using CarApi.Models;
using Microsoft.EntityFrameworkCore;
using TransactionsApi.Models;

namespace TransactionsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<SpendingsApi.Models.UserCars> UserCars { get; set; }
        public DbSet<Car> Car { get; set; }
    }
}

