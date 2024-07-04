using lyricboxd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lyricboxd.Data
{
    public class LyricboxdDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public LyricboxdDbContext(DbContextOptions<LyricboxdDbContext> options)
            : base(options)
        {
        }

        public DbSet<Review> Reviews { get; set; }
    }
}
