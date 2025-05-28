using LandingPage.Models;
using Microsoft.EntityFrameworkCore;

namespace LandingPage.Data
{
    public class STMprocessDB : DbContext
    {
        public STMprocessDB(DbContextOptions<STMprocessDB> options) : base(options) { }

        public DbSet<ProcessDatabase> ProcessDatabase { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
