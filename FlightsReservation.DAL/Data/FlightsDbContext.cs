using FlightsReservation.DAL.Configuration;
using FlightsReservation.DAL.Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightsReservation.DAL.Data
{
    public class FlightsDbContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Passenger> Passengers { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;

        public FlightsDbContext(DbContextOptions<FlightsDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FlightConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new PassengerConfiguration());
            modelBuilder.ApplyConfiguration(new SeatConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
