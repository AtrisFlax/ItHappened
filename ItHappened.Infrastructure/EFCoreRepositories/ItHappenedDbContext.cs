using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class ItHappenedDbContext : DbContext
    {
        public DbSet<UserDB> Users { get; set; }
        
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GeoTag> GeoTags { get; set; }

        public DbSet<Photo> Photos { get; set; }

        //public DbSet<EventCustomParameters> CustomParameters { get; set; }
        //public DbSet<Event> Events { get; set; }
        //public DbSet<EventCustomParameters> CustomParameters { get; set; }
    

        public ItHappenedDbContext(DbContextOptions<ItHappenedDbContext> options) : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDB>().ToTable("Users", "ItHappenedDB");
        }
    }
}