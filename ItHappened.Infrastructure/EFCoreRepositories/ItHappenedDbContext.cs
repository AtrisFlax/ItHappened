using ItHappened.Domain;
using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class ItHappenedDbContext : DbContext
    {
        public ItHappenedDbContext(DbContextOptions<ItHappenedDbContext> options) : base(options) { }
        
        public DbSet<Event> Events { get; set; }
    }
}