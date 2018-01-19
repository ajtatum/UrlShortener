using Microsoft.EntityFrameworkCore;

namespace UrlShortener.DAL
{
    public class PlaygroundContext : DbContext
    {
        public PlaygroundContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Models.UrlShortener> UrlShorteners { get; set; }
    }
}
