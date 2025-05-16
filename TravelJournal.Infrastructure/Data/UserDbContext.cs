using Microsoft.EntityFrameworkCore;
using TravelJournal.Domain.Models;
using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Infrastructure.Data;

//
// obsolete class (not currently used)
//

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        });
    }
}