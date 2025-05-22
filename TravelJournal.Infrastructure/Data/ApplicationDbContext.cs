using Microsoft.EntityFrameworkCore;
using TravelJournal.Domain.Models;
using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : DbContext(context)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Route> Routes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: === Users ===
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            
            entity.Property(u => u.Id)
                .HasColumnName("id");

            entity.Property(u => u.Name)
                .HasColumnName("name");
            
            entity.Property(u => u.Email)
                .HasConversion(vo => vo.Value, str => Email.Create(str))
                .HasColumnName("email");

            entity.Property(u => u.CreatedAt)
                .HasColumnName("created_at");
                
            entity.HasMany(u => u.Trips)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // TODO: === Trips ===
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
                
            entity.Property(t => t.UserId)
                .HasColumnName("user_id");

            entity.OwnsOne(t => t.Status, status =>
            {
                status.Property(s => s.Value)
                    .HasColumnName("status")
                    .HasMaxLength(50);
            });
            
            entity.HasOne(t => t.User)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasMany(t => t.Routes)
                .WithOne(r => r.Trip)
                .HasForeignKey(r => r.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // TODO: === Routes ===
        modelBuilder.Entity<Route>(entity =>
        {
            entity.ToTable("routes");

            entity.Property(r => r.Id)
                .HasColumnName("id");
            
            entity.Property(r => r.LocationName)
                .HasColumnName("location_name");

            entity.Property(r => r.Country)
                .HasColumnName("country");

            entity.Property(r => r.City)
                .HasColumnName("city");
                
            entity.Property(r => r.TripId)
                .HasColumnName("trip_id");
            
            entity.HasOne(r => r.Trip)
                .WithMany(t => t.Routes)
                .HasForeignKey(r => r.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}