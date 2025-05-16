using Microsoft.EntityFrameworkCore;
using TravelJournal.Domain.Models;

namespace TravelJournal.Infrastructure.Data;

//
// obsolete class (not currently used)
//

public class TripDbContext(DbContextOptions<TripDbContext> options) : DbContext(options)
{
    public DbSet<Trip> Trips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.ToTable("trips");

            entity.Property(t => t.Id)
                .HasColumnName("trip_id");

            entity.Property(t => t.StartDate)
                .HasColumnName("start_date");

            entity.Property(t => t.EndDate)
                .HasColumnName("end_date");

            entity.Property(t => t.CreatedAt)
                .HasColumnName("created_at");

            entity.Property(t => t.UpdatedAt)
                .HasColumnName("update_at");

            entity.OwnsOne(t => t.Status, status =>
            {
                status.Property(s => s.Value)
                    .HasColumnName("status")
                    .HasMaxLength(50);
            });
        });
    }
}