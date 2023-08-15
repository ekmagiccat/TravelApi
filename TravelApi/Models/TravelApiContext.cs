using Microsoft.EntityFrameworkCore;

namespace TravelApi.Models
{
    public class TravelApiContext : DbContext
    {
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public TravelApiContext(DbContextOptions<TravelApiContext> options) : base(options)
        {
        }
    }
}