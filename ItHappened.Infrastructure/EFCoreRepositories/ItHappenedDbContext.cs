using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class ItHappenedDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ItHappenedDbContext(DbContextOptions<ItHappenedDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users", "ItHappenedDB");
        }
    }
}