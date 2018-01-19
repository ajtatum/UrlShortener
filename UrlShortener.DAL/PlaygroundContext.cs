using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.DAL
{
    public class PlaygroundContext : IdentityDbContext<ApplicationUser>
    {
        public PlaygroundContext(DbContextOptions options) : base(options)
        { }

        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
