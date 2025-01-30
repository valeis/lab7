using Microsoft.EntityFrameworkCore;

namespace lab7.Models
{
    public class TrackContext:DbContext
    {
        public TrackContext(DbContextOptions<TrackContext> options)
           : base(options)
        {
        }
        public DbSet<Track> Track { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Ad> Ad { get; set; }
    }
}
