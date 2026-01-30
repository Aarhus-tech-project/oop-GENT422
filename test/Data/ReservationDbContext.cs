using Microsoft.EntityFrameworkCore;
using ReservationSystem.Models;

public class ReservationDbContext : DbContext
{
    public ReservationDbContext(DbContextOptions<ReservationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Restaurant> Restaurant    { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Airplane> Airplanes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

      
        modelBuilder.Entity<Venue>()
            .HasDiscriminator<string>("VenueType")
            .HasValue<Restaurant>("Restaurant")
            .HasValue<Cinema>("Cinema")
            .HasValue<Airplane>("Airplane");

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Venue)
            .WithMany()
            .HasForeignKey(r => r.VenueId)
            .OnDelete(DeleteBehavior.NoAction);

        

        modelBuilder.Entity<Seat>()
            .HasOne(s => s.Venue)
            .WithMany()
            .HasForeignKey(s => s.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
