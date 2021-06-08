using Microsoft.EntityFrameworkCore;

namespace CostsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CostsApi.Models.Costs> Costs { get; set; }
    }
}
