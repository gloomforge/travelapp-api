using Microsoft.EntityFrameworkCore;
using TravelJournal.Domain.Models;

namespace TravelJournal.Infrastructure.Data;

//
// obsolete class (not currently used)
//

public class RouteDbContext(DbContextOptions<RouteDbContext> options) : DbContext(options)
{
    public DbSet<Route> Routes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Route>(entity =>
        {
            entity.ToTable("routes");

            entity.Property(t => t.LocationName)
                .HasColumnName("route_id");
            
            entity.Property(t => t.LocationName)
                .HasColumnName("location_name");

            entity.Property(t => t.Country)
                .HasColumnName("country");

            entity.Property(t => t.City)
                .HasColumnName("city");
        });
    }
}