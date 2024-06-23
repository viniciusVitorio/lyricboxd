using lyricboxd.Models;
using Microsoft.EntityFrameworkCore;

namespace lyricboxd.Data
{
    public class LyricboxdDbContext : DbContext
    {
        public LyricboxdDbContext(DbContextOptions<LyricboxdDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Review> Reviews { get; set; }
    }
}
