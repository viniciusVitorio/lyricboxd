using Microsoft.EntityFrameworkCore;
using lyricboxd.Models;

namespace lyricboxd.Data
{
    public class LyricboxdDbContext : DbContext
    {
        public LyricboxdDbContext(DbContextOptions<LyricboxdDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
